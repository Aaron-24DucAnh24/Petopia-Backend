using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Admin;
using Petopia.Business.Models.Common;
using Petopia.Business.Models.Enums;
using Petopia.Business.Utils;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
  public class AdminService : BaseService, IAdminService
  {
    public AdminService(
      IServiceProvider provider,
      ILogger<AdminService> logger
    ) : base(provider, logger)
    {
    }

    public async Task<bool> ActivateUserAsync(ActivateRequestModel request)
    {
      bool result = true;

      if (request.Type == "user")
      {
        User user = await UnitOfWork.Users
          .AsTracking()
          .FirstAsync(u => u.Id == request.Id);
        user.IsDeactivated = !user.IsDeactivated;
        UnitOfWork.Users.Update(user);
        result = user.IsDeactivated;
      }

      if (request.Type == "pet")
      {
        Pet pet = await UnitOfWork.Pets
          .AsTracking()
          .FirstAsync(u => u.Id == request.Id);
        pet.IsDeleted = !pet.IsDeleted;
        UnitOfWork.Pets.Update(pet);
        result = pet.IsDeleted;
      }

      if (request.Type == "blog")
      {
        Blog blog = await UnitOfWork.Blogs
          .AsTracking()
          .FirstAsync(u => u.Id == request.Id);
        blog.IsHidden = !blog.IsHidden;
        UnitOfWork.Blogs.Update(blog);
        result = blog.IsHidden;
      }

      await UnitOfWork.SaveChangesAsync();
      return result;
    }

    public async Task<string> CreateAdminAsync(string email)
    {
      string password = string.Empty;
      User? user = await UnitOfWork.Users
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Email == HashUtils.EnryptString(email));
      if (user == null)
      {
        password = TokenUtils.GetRandomPassword();
        user = await UnitOfWork.Users.CreateAsync(new User()
        {
          Id = Guid.NewGuid(),
          Email = HashUtils.EnryptString(email),
          Password = HashUtils.HashPassword(password),
          IsCreatedAt = DateTimeOffset.Now,
          Role = UserRole.SystemAdmin,
        });
        await UnitOfWork.UserIndividualAttributes.CreateAsync(new UserIndividualAttributes()
        {
          Id = user.Id,
          FirstName = "New",
          LastName = "Admin"
        });
      }
      else
      {
        user.Role = UserRole.SystemAdmin;
        UnitOfWork.Users.Update(user);
      }
      await UnitOfWork.SaveChangesAsync();
      return password;
    }

    async public Task<DashboardReponseModel> GetDashboardAsync(DashboardRequestModel time)
    {
      int income = (await UnitOfWork.Payments
        .Where(x => x.IsCreatedAt.Year == time.Year && x.IsCreatedAt.Month == time.Month)
        .ToListAsync()).Sum(x => x.Amount);
      int petNumber = await UnitOfWork.Pets.CountAsync(x => !x.IsDeleted);
      int individualNumber = await UnitOfWork.Users.CountAsync(x => x.Role == UserRole.StandardUser && !x.IsDeactivated);
      int organizationNumber = await UnitOfWork.Users.CountAsync(x => x.Role == UserRole.Organization && !x.IsDeactivated);
      int blogNumber = await UnitOfWork.Blogs.CountAsync(x => !x.IsHidden);
      int activeUserNumber = await UnitOfWork.UserConnections
        .Include(x => x.User)
        .Where(x => x.AccessTokenExpirationDate.AddDays(-7).Year == time.Year && x.AccessTokenExpirationDate.AddDays(-7).Month == time.Month)
        .Where(x => !x.User.IsDeactivated && x.User.Role != UserRole.SystemAdmin)
        .CountAsync();

      List<int> petData = new();
      List<int> blogData = new();
      List<int> adoptionData = new();
      List<Task> tasks = new();
      int finalDay = time.Month == DateTime.Today.Month
        ? DateTime.Today.Day
        : DateTime.DaysInMonth(time.Year, time.Month);

      for (int day = 1; day <= finalDay; day++)
      {
        await GetChartDataAsync(petData, blogData, adoptionData, time, day);
      }

      return new DashboardReponseModel()
      {
        Income = income,
        PetNumber = petNumber,
        BlogNumber = blogNumber,
        IndividualNumber = individualNumber,
        OrganizationNumber = organizationNumber,
        ActiveRate = (int)(activeUserNumber / (float)(individualNumber + organizationNumber) * 100),
        PetData = petData,
        BlogData = blogData,
        AdoptionData = adoptionData,
      };
    }

    async public Task<PaginationResponseModel<ManagementUserResponseModel>> GetUsersAsync(PaginationRequestModel<ManagementUserFilter> request)
    {
      IQueryable<User> query = UnitOfWork.Users
        .Include(x => x.UserIndividualAttributes)
        .Include(x => x.UserOrganizationAttributes)
        .Where(x => x.Role == request.Filter.Role)
        .Distinct()
        .OrderBy(x => x.IsCreatedAt)
        .AsQueryable();

      return await PagingAsync<ManagementUserResponseModel, User>(query, request);
    }

    public async Task<PaginationResponseModel<ManagementPetResponseModel>> GetPetsAsync(PaginationRequestModel request)
    {
      IQueryable<Pet> query = UnitOfWork.Pets
        .Include(x => x.Owner)
        .Include(x => x.Images)
        .Distinct()
        .OrderBy(x => x.IsCreatedAt)
        .AsQueryable();

      return await PagingAsync<ManagementPetResponseModel, Pet>(query, request);
    }

    public async Task<PaginationResponseModel<ManagementBlogResponseModel>> GetBlogsAsync(PaginationRequestModel request)
    {
      IQueryable<Blog> query = UnitOfWork.Blogs
        .Include(x => x.User)
        .Distinct()
        .OrderBy(x => x.IsCreatedAt)
        .AsQueryable();

      return await PagingAsync<ManagementBlogResponseModel, Blog>(query, request);
    }

    public async Task<PaginationResponseModel<ManagementUpgradeResponseModel>> GetUpgradeRequestsAsync(PaginationRequestModel request)
    {
      IQueryable<UpgradeForm> query = UnitOfWork.UpgradeForms
        .Where(x => x.Status == UpgradeStatus.Pending)
        .Include(x => x.User)
        .Distinct()
        .OrderBy(x => x.IsCreatedAt)
        .AsQueryable();

      return await PagingAsync<ManagementUpgradeResponseModel, UpgradeForm>(query, request);
    }

    public async Task<string> ActUpgradeRequestAsync(Guid id, bool accepted)
    {
      UpgradeForm form = await UnitOfWork.UpgradeForms
        .AsTracking()
        .Include(x => x.User)
        .FirstAsync(x => x.Id == id);

      if (!accepted)
      {
        form.Status = UpgradeStatus.Rejected;
        UnitOfWork.UpgradeForms.Update(form);
        await UnitOfWork.SaveChangesAsync();
        return HashUtils.DecryptString(form.User.Email);
      }

      await UnitOfWork.UserOrganizationAttributes.CreateAsync(new UserOrganizationAttributes()
      {
        Id = form.UserId,
        EntityName = form.EntityName,
        OrganizationName = form.OrganizationName,
        Email = HashUtils.EnryptString(form.Email),
        Website = form.Website,
        TaxCode = form.TaxCode,
        Type = form.Type,
        Description = form.Description,
      });

      form.User.Role = UserRole.Organization;
      form.User.Phone = form.Phone;
      form.User.ProvinceCode = form.PrivinceCode;
      form.User.DistrictCode = form.DistrictCode;
      form.User.WardCode = form.WardCode;
      form.User.Street = form.Street;
      form.User.Address = form.Address;
      form.Status = UpgradeStatus.Accepted;

      UnitOfWork.UpgradeForms.Update(form);
      await UnitOfWork.SaveChangesAsync();

      return HashUtils.DecryptString(form.User.Email);
    }

    public async Task<PaginationResponseModel<ManagementReportResponseModel>> GetReportsAsync(PaginationRequestModel<ManagementReportFilter> request)
    {
      List<ManagementReportResponseModel> result = new();
      int scam = 0;
      int spam = 0;
      int other = 0;
      int unappropriate = 0;

      if (request.Filter.ReportEntity == ReportEntity.User)
      {
        List<Guid?> ids = await UnitOfWork.Reports
          .AsQueryable()
          .Select(x => x.UserId)
          .Distinct()
          .ToListAsync();
        ids.Sort();

        foreach (var id in ids)
        {
          if (id != null)
          {
            scam = await UnitOfWork.Reports.CountAsync(x => x.UserId == id && x.Type == ReportType.Scam);
            spam = await UnitOfWork.Reports.CountAsync(x => x.UserId == id && x.Type == ReportType.Spam);
            other = await UnitOfWork.Reports.CountAsync(x => x.UserId == id && x.Type == ReportType.Other);
            unappropriate = await UnitOfWork.Reports.CountAsync(x => x.UserId == id && x.Type == ReportType.InappropriateContent);
            result.Add(new ManagementReportResponseModel()
            {
              Id = id,
              Spam = spam,
              Scam = scam,
              Other = other,
              InappropriateContent = unappropriate,
            });
          }
        }
      }

      if (request.Filter.ReportEntity == ReportEntity.Pet)
      {
        List<Guid?> ids = await UnitOfWork.Reports
          .AsQueryable()
          .Select(x => x.PetId)
          .Distinct()
          .ToListAsync();
        ids.Sort();

        foreach (var id in ids)
        {
          if (id != null)
          {
            scam = await UnitOfWork.Reports.CountAsync(x => x.PetId == id && x.Type == ReportType.Scam);
            spam = await UnitOfWork.Reports.CountAsync(x => x.PetId == id && x.Type == ReportType.Spam);
            other = await UnitOfWork.Reports.CountAsync(x => x.PetId == id && x.Type == ReportType.Other);
            unappropriate = await UnitOfWork.Reports.CountAsync(x => x.PetId == id && x.Type == ReportType.InappropriateContent);
            result.Add(new ManagementReportResponseModel()
            {
              Id = id,
              Spam = spam,
              Scam = scam,
              Other = other,
              InappropriateContent = unappropriate,
            });
          }
        }
      }

      if (request.Filter.ReportEntity == ReportEntity.Blog)
      {
        List<Guid?> ids = await UnitOfWork.Reports
          .AsQueryable()
          .Select(x => x.BlogId)
          .Distinct()
          .ToListAsync();
        ids.Sort();

        foreach (var id in ids)
        {
          if (id != null)
          {
            scam = await UnitOfWork.Reports.CountAsync(x => x.BlogId == id && x.Type == ReportType.Scam);
            spam = await UnitOfWork.Reports.CountAsync(x => x.BlogId == id && x.Type == ReportType.Spam);
            other = await UnitOfWork.Reports.CountAsync(x => x.BlogId == id && x.Type == ReportType.Other);
            unappropriate = await UnitOfWork.Reports.CountAsync(x => x.BlogId == id && x.Type == ReportType.InappropriateContent);
            result.Add(new ManagementReportResponseModel()
            {
              Id = id,
              Spam = spam,
              Scam = scam,
              Other = other,
              InappropriateContent = unappropriate,
            });
          }
        }
      }

      return ListPaging<ManagementReportResponseModel, ManagementReportResponseModel>(result, request);
    }

    private async Task GetChartDataAsync(
      List<int> petData,
      List<int> blogData,
      List<int> adoptionData,
      DashboardRequestModel time,
      int day
    )
    {
      petData.Add(
        await UnitOfWork.Pets.CountAsync(x => x.IsCreatedAt.Day == day
          && x.IsCreatedAt.Month == time.Month
          && x.IsUpdatedAt.Year == time.Year));
      blogData.Add(
        await UnitOfWork.Blogs.CountAsync(x => x.IsCreatedAt.Day == day
          && x.IsCreatedAt.Month == time.Month
          && x.IsUpdatedAt.Year == time.Year));
      adoptionData.Add(
        await UnitOfWork.AdoptionForms.CountAsync(x => x.IsCreatedAt.Day == day
          && x.IsCreatedAt.Month == time.Month
          && x.IsUpdatedAt.Year == time.Year
          && x.Status == AdoptStatus.Adopted));
    }
  }
}