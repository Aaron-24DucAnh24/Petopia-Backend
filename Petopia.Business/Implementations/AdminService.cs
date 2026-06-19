using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Admin;
using Petopia.Business.Models.Common;
using Petopia.Business.Models.Enums;
using Petopia.Business.Models.Exceptions;
using Petopia.Business.Utils;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
  public class AdminService : BaseService, IAdminService
  {
    public AdminService(IServiceProvider provider, ILogger<AdminService> logger) : base(provider, logger)
    {
    }

    public async Task<AdminStatisticsResponseModel> GetStatisticsAsync()
    {
      var now = DateTimeOffset.UtcNow;

      var totalUsers = await UnitOfWork.Users.CountAsync(_ => true);
      var activeUsers = await UnitOfWork.Users.CountAsync(x => !x.IsDeactivated);
      var totalOrganizations = await UnitOfWork.Users.CountAsync(x => x.Role == UserRole.Organization);
      var totalPets = await UnitOfWork.Pets.CountAsync(x => !x.IsDeleted);
      var availablePets = await UnitOfWork.Pets.CountAsync(x => !x.IsDeleted && x.IsAvailable);
      var adoptedPets = await UnitOfWork.Pets.CountAsync(x => !x.IsDeleted && !x.IsAvailable);
      var totalPosts = await UnitOfWork.Posts.CountAsync(x => !x.IsDeleted);
      var totalBlogs = await UnitOfWork.Blogs.CountAsync(_ => true);

      var payments = await UnitOfWork.Payments.AsQueryable().ToListAsync();
      var pendingReports = await UnitOfWork.Reports.CountAsync(x => !x.IsResolved);
      var resolvedReports = await UnitOfWork.Reports.CountAsync(x => x.IsResolved);

      var monthlyStats = new List<AdminMonthlyStatsModel>();
      for (int i = 5; i >= 0; i--)
      {
        var monthStart = new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, TimeSpan.Zero).AddMonths(-i);
        var monthEnd = monthStart.AddMonths(1);
        monthlyStats.Add(new AdminMonthlyStatsModel
        {
          Month = monthStart.ToString("MM/yyyy"),
          Users = await UnitOfWork.Users.CountAsync(x => x.IsCreatedAt >= monthStart && x.IsCreatedAt < monthEnd),
          Pets = await UnitOfWork.Pets.CountAsync(x => x.IsCreatedAt >= monthStart && x.IsCreatedAt < monthEnd),
          Posts = await UnitOfWork.Posts.CountAsync(x => x.IsCreatedAt >= monthStart && x.IsCreatedAt < monthEnd),
          Payments = payments.Count(x => x.IsCreatedAt >= monthStart && x.IsCreatedAt < monthEnd),
        });
      }

      return new AdminStatisticsResponseModel
      {
        TotalUsers = totalUsers,
        ActiveUsers = activeUsers,
        TotalOrganizations = totalOrganizations,
        TotalPets = totalPets,
        AvailablePets = availablePets,
        AdoptedPets = adoptedPets,
        TotalPosts = totalPosts,
        TotalBlogs = totalBlogs,
        TotalPayments = payments.Count,
        TotalRevenue = payments.Sum(x => (long)x.Amount),
        PendingReports = pendingReports,
        ResolvedReports = resolvedReports,
        MonthlyStats = monthlyStats,
      };
    }

    public async Task<PaginationResponseModel<ManagementUserResponseModel>> GetUsersAsync(
      PaginationRequestModel<AdminSearchFilterModel> request)
    {
      var keyword = request.Filter.Keyword?.ToLower();
      var isActive = request.Filter.IsActive;

      var query = UnitOfWork.Users
        .Include(x => x.UserIndividualAttributes)
        .Include(x => x.UserOrganizationAttributes)
        .AsQueryable()
        .AsSplitQuery();

      if (!string.IsNullOrEmpty(keyword))
      {
        query = query.Where(x =>
          (x.UserIndividualAttributes != null &&
           (x.UserIndividualAttributes.FirstName.ToLower().Contains(keyword) ||
            x.UserIndividualAttributes.LastName.ToLower().Contains(keyword))) ||
          (x.UserOrganizationAttributes != null &&
           x.UserOrganizationAttributes.OrganizationName.ToLower().Contains(keyword)) ||
          (x.Phone != null && x.Phone.Contains(keyword)));
      }

      if (isActive.HasValue)
      {
        query = query.Where(x => x.IsDeactivated == !isActive.Value);
      }

      query = query.OrderByDescending(x => x.IsCreatedAt);

      var total = await query.CountAsync();
      var pageSize = request.PageSize > 0 ? request.PageSize : 10;
      var pageNumber = (int)Math.Ceiling((double)total / pageSize);
      pageNumber = Math.Max(1, pageNumber);
      var pageIndex = Math.Clamp(request.PageIndex, 1, pageNumber);

      var users = await query
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

      var data = users.Select(x => new ManagementUserResponseModel
      {
        Id = x.Id,
        Image = x.Image ?? string.Empty,
        Name = x.Role == UserRole.Organization
          ? (x.UserOrganizationAttributes?.OrganizationName ?? string.Empty)
          : string.Join(" ", x.UserIndividualAttributes?.FirstName ?? string.Empty, x.UserIndividualAttributes?.LastName ?? string.Empty).Trim(),
        Email = HashUtils.DecryptString(x.Email),
        Phone = x.Phone ?? string.Empty,
        Role = x.Role,
        IsCreatedAt = x.IsCreatedAt,
        IsActive = !x.IsDeactivated,
      }).ToList();

      return new PaginationResponseModel<ManagementUserResponseModel>
      {
        Data = data,
        TotalNumber = total,
        PageIndex = pageIndex,
        PageSize = pageSize,
        PageNumber = pageNumber,
      };
    }

    public async Task<bool> DeactivateUserAsync(Guid id)
    {
      var user = await UnitOfWork.Users.AsTracking().FirstOrDefaultAsync(x => x.Id == id)
        ?? throw new UserNotFoundException();
      user.IsDeactivated = true;
      UnitOfWork.Users.Update(user);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ActivateUserAsync(Guid id)
    {
      var user = await UnitOfWork.Users.AsTracking().FirstOrDefaultAsync(x => x.Id == id)
        ?? throw new UserNotFoundException();
      user.IsDeactivated = false;
      UnitOfWork.Users.Update(user);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<PaginationResponseModel<ManagementPetResponseModel>> GetPetsAsync(
      PaginationRequestModel<AdminSearchFilterModel> request)
    {
      var keyword = request.Filter.Keyword?.ToLower();
      var isActive = request.Filter.IsActive;

      var query = UnitOfWork.Pets
        .Include(x => x.Owner)
        .ThenInclude(o => o.UserIndividualAttributes)
        .Include(x => x.Owner)
        .ThenInclude(o => o.UserOrganizationAttributes)
        .AsQueryable()
        .AsSplitQuery();

      if (!string.IsNullOrEmpty(keyword))
        query = query.Where(x => x.Name.ToLower().Contains(keyword) || x.Breed.ToLower().Contains(keyword));

      if (isActive.HasValue)
        query = query.Where(x => x.IsDeleted == !isActive.Value);

      query = query.OrderByDescending(x => x.IsCreatedAt);

      return await BuildPagedResponse(query, request, x => new ManagementPetResponseModel
      {
        Id = x.Id,
        Name = x.Name,
        Species = x.Species,
        Breed = x.Breed,
        IsAvailable = x.IsAvailable,
        IsActive = !x.IsDeleted,
        OwnerId = x.OwnerId,
        OwnerName = x.Owner.Role == UserRole.Organization
          ? (x.Owner.UserOrganizationAttributes?.OrganizationName ?? string.Empty)
          : string.Join(" ", x.Owner.UserIndividualAttributes?.FirstName ?? string.Empty, x.Owner.UserIndividualAttributes?.LastName ?? string.Empty).Trim(),
        IsCreatedAt = x.IsCreatedAt,
      });
    }

    public async Task<bool> DeactivatePetAsync(Guid id)
    {
      var pet = await UnitOfWork.Pets.AsTracking().FirstOrDefaultAsync(x => x.Id == id)
        ?? throw new PetNotFoundException();
      pet.IsDeleted = true;
      UnitOfWork.Pets.Update(pet);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ActivatePetAsync(Guid id)
    {
      var pet = await UnitOfWork.Pets.AsTracking().FirstOrDefaultAsync(x => x.Id == id)
        ?? throw new PetNotFoundException();
      pet.IsDeleted = false;
      UnitOfWork.Pets.Update(pet);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<PaginationResponseModel<ManagementPostResponseModel>> GetPostsAsync(
      PaginationRequestModel<AdminSearchFilterModel> request)
    {
      var keyword = request.Filter.Keyword?.ToLower();
      var isActive = request.Filter.IsActive;

      var query = UnitOfWork.Posts
        .Include(x => x.User)
        .ThenInclude(u => u.UserIndividualAttributes)
        .Include(x => x.User)
        .ThenInclude(u => u.UserOrganizationAttributes)
        .AsQueryable()
        .AsSplitQuery();

      if (!string.IsNullOrEmpty(keyword))
        query = query.Where(x => x.Content.ToLower().Contains(keyword));

      if (isActive.HasValue)
        query = query.Where(x => x.IsDeleted == !isActive.Value);

      query = query.OrderByDescending(x => x.IsCreatedAt);

      return await BuildPagedResponse(query, request, x => new ManagementPostResponseModel
      {
        Id = x.Id,
        UserId = x.UserId,
        UserName = x.User.Role == UserRole.Organization
          ? (x.User.UserOrganizationAttributes?.OrganizationName ?? string.Empty)
          : string.Join(" ", x.User.UserIndividualAttributes?.FirstName ?? string.Empty, x.User.UserIndividualAttributes?.LastName ?? string.Empty).Trim(),
        Content = x.Content,
        Like = x.Like,
        IsActive = !x.IsDeleted,
        IsCreatedAt = x.IsCreatedAt,
      });
    }

    public async Task<bool> DeactivatePostAsync(Guid id)
    {
      var post = await UnitOfWork.Posts.AsTracking().FirstOrDefaultAsync(x => x.Id == id)
        ?? throw new PostNotFoundException();
      post.IsDeleted = true;
      UnitOfWork.Posts.Update(post);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ActivatePostAsync(Guid id)
    {
      var post = await UnitOfWork.Posts.AsTracking().FirstOrDefaultAsync(x => x.Id == id)
        ?? throw new PostNotFoundException();
      post.IsDeleted = false;
      UnitOfWork.Posts.Update(post);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<PaginationResponseModel<ManagementBlogResponseModel>> GetBlogsAsync(
      PaginationRequestModel<AdminSearchFilterModel> request)
    {
      var keyword = request.Filter.Keyword?.ToLower();
      var isActive = request.Filter.IsActive;

      var query = UnitOfWork.Blogs
        .Include(x => x.User)
        .ThenInclude(u => u.UserIndividualAttributes)
        .Include(x => x.User)
        .ThenInclude(u => u.UserOrganizationAttributes)
        .AsQueryable()
        .AsSplitQuery();

      if (!string.IsNullOrEmpty(keyword))
        query = query.Where(x => x.Title.ToLower().Contains(keyword));

      if (isActive.HasValue)
        query = query.Where(x => x.IsHidden == !isActive.Value);

      query = query.OrderByDescending(x => x.IsCreatedAt);

      return await BuildPagedResponse(query, request, x => new ManagementBlogResponseModel
      {
        Id = x.Id,
        UserId = x.UserId,
        UserName = x.User.Role == UserRole.Organization
          ? (x.User.UserOrganizationAttributes?.OrganizationName ?? string.Empty)
          : string.Join(" ", x.User.UserIndividualAttributes?.FirstName ?? string.Empty, x.User.UserIndividualAttributes?.LastName ?? string.Empty).Trim(),
        Title = x.Title,
        Category = x.Category,
        View = x.View,
        IsActive = !x.IsHidden,
        IsCreatedAt = x.IsCreatedAt,
      });
    }

    public async Task<bool> DeactivateBlogAsync(Guid id)
    {
      var blog = await UnitOfWork.Blogs.AsTracking().FirstOrDefaultAsync(x => x.Id == id)
        ?? throw new BlogNotFoundException();
      blog.IsHidden = true;
      UnitOfWork.Blogs.Update(blog);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ActivateBlogAsync(Guid id)
    {
      var blog = await UnitOfWork.Blogs.AsTracking().FirstOrDefaultAsync(x => x.Id == id)
        ?? throw new BlogNotFoundException();
      blog.IsHidden = false;
      UnitOfWork.Blogs.Update(blog);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<PaginationResponseModel<ManagementPaymentResponseModel>> GetPaymentsAsync(
      PaginationRequestModel<AdminSearchFilterModel> request)
    {
      var keyword = request.Filter.Keyword?.ToLower();

      var query = UnitOfWork.Payments
        .Include(x => x.Blog)
        .ThenInclude(b => b.User)
        .ThenInclude(u => u.UserIndividualAttributes)
        .Include(x => x.Blog)
        .ThenInclude(b => b.User)
        .ThenInclude(u => u.UserOrganizationAttributes)
        .AsQueryable()
        .AsSplitQuery();

      if (!string.IsNullOrEmpty(keyword))
        query = query.Where(x => x.Blog.Title.ToLower().Contains(keyword));

      query = query.OrderByDescending(x => x.IsCreatedAt);

      return await BuildPagedResponse(query, request, x => new ManagementPaymentResponseModel
      {
        Id = x.Id,
        BlogId = x.BlogId,
        BlogTitle = x.Blog.Title,
        UserName = x.Blog.User.Role == UserRole.Organization
          ? (x.Blog.User.UserOrganizationAttributes?.OrganizationName ?? string.Empty)
          : string.Join(" ", x.Blog.User.UserIndividualAttributes?.FirstName ?? string.Empty, x.Blog.User.UserIndividualAttributes?.LastName ?? string.Empty).Trim(),
        Amount = x.Amount,
        MonthDuration = (int)Math.Round((x.AdvertisingDate - x.IsCreatedAt).TotalDays / 30),
        IsCreatedAt = x.IsCreatedAt,
      });
    }

    public async Task<PaginationResponseModel<ManagementReportResponseModel>> GetReportsAsync(
      PaginationRequestModel<AdminSearchFilterModel> request)
    {
      var isActive = request.Filter.IsActive;

      var allReports = await UnitOfWork.Reports.AsQueryable().ToListAsync();

      var blogGroups = allReports
        .Where(r => r.BlogId.HasValue)
        .GroupBy(r => r.BlogId!.Value)
        .Select(g => new ManagementReportResponseModel
        {
          TargetId = g.Key,
          TargetType = ReportEntity.Blog,
          SpamCount = g.Count(r => r.Type == ReportType.Spam),
          ScamCount = g.Count(r => r.Type == ReportType.Scam),
          InappropriateCount = g.Count(r => r.Type == ReportType.InappropriateContent),
          OtherCount = g.Count(r => r.Type == ReportType.Other),
          TotalCount = g.Count(),
          IsResolved = g.All(r => r.IsResolved),
          LastReportAt = g.Max(r => r.IsCreatedAt),
        });

      var userGroups = allReports
        .Where(r => r.UserId.HasValue)
        .GroupBy(r => r.UserId!.Value)
        .Select(g => new ManagementReportResponseModel
        {
          TargetId = g.Key,
          TargetType = ReportEntity.User,
          SpamCount = g.Count(r => r.Type == ReportType.Spam),
          ScamCount = g.Count(r => r.Type == ReportType.Scam),
          InappropriateCount = g.Count(r => r.Type == ReportType.InappropriateContent),
          OtherCount = g.Count(r => r.Type == ReportType.Other),
          TotalCount = g.Count(),
          IsResolved = g.All(r => r.IsResolved),
          LastReportAt = g.Max(r => r.IsCreatedAt),
        });

      var petGroups = allReports
        .Where(r => r.PetId.HasValue)
        .GroupBy(r => r.PetId!.Value)
        .Select(g => new ManagementReportResponseModel
        {
          TargetId = g.Key,
          TargetType = ReportEntity.Pet,
          SpamCount = g.Count(r => r.Type == ReportType.Spam),
          ScamCount = g.Count(r => r.Type == ReportType.Scam),
          InappropriateCount = g.Count(r => r.Type == ReportType.InappropriateContent),
          OtherCount = g.Count(r => r.Type == ReportType.Other),
          TotalCount = g.Count(),
          IsResolved = g.All(r => r.IsResolved),
          LastReportAt = g.Max(r => r.IsCreatedAt),
        });

      var combined = blogGroups
        .Concat(userGroups)
        .Concat(petGroups)
        .OrderByDescending(r => r.LastReportAt)
        .ToList();

      if (isActive.HasValue)
        combined = combined.Where(r => r.IsResolved == !isActive.Value).ToList();

      var total = combined.Count;
      var pageSize = request.PageSize > 0 ? request.PageSize : 10;
      var pageNumber = Math.Max(1, (int)Math.Ceiling((double)total / pageSize));
      var pageIndex = Math.Clamp(request.PageIndex, 1, pageNumber);

      return new PaginationResponseModel<ManagementReportResponseModel>
      {
        Data = combined.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
        TotalNumber = total,
        PageIndex = pageIndex,
        PageSize = pageSize,
        PageNumber = pageNumber,
      };
    }

    public async Task<bool> ResolveReportAsync(ResolveReportRequestModel request)
    {
      var reports = request.TargetType switch
      {
        ReportEntity.Blog => await UnitOfWork.Reports.AsTracking().Where(x => x.BlogId == request.TargetId).ToListAsync(),
        ReportEntity.User => await UnitOfWork.Reports.AsTracking().Where(x => x.UserId == request.TargetId).ToListAsync(),
        ReportEntity.Pet => await UnitOfWork.Reports.AsTracking().Where(x => x.PetId == request.TargetId).ToListAsync(),
        _ => new List<Report>(),
      };

      foreach (var report in reports)
      {
        report.IsResolved = true;
        UnitOfWork.Reports.Update(report);
      }

      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    private static async Task<PaginationResponseModel<TResult>> BuildPagedResponse<TEntity, TResult>(
      IQueryable<TEntity> query,
      PaginationRequestModel request,
      Func<TEntity, TResult> selector)
    {
      var total = await query.CountAsync();
      var pageSize = request.PageSize > 0 ? request.PageSize : 10;
      var pageNumber = Math.Max(1, (int)Math.Ceiling((double)total / pageSize));
      var pageIndex = Math.Clamp(request.PageIndex, 1, pageNumber);

      var items = await query
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

      return new PaginationResponseModel<TResult>
      {
        Data = items.Select(selector).ToList(),
        TotalNumber = total,
        PageIndex = pageIndex,
        PageSize = pageSize,
        PageNumber = pageNumber,
      };
    }
  }
}
