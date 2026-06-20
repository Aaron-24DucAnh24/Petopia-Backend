using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Enums;
using Petopia.Business.Models.Notification;
using Petopia.Business.Models.Report;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
  public class ReportService : BaseService, IReportService
  {
    private readonly INotificationService _notificationService;

    public ReportService(IServiceProvider provider, ILogger<ReportService> logger) : base(provider, logger)
    {
      _notificationService = provider.GetRequiredService<INotificationService>();
    }

    public async Task<bool> PreReportAsync(PreReportRequestModel request)
    {
      if (request.Entity == ReportEntity.Blog)
      {
        if (!await UnitOfWork.Blogs.AnyAsync(x => x.Id == request.Id && x.UserId != UserContext.Id))
        {
          return false;
        }
        return !await UnitOfWork.Reports.AnyAsync(x => x.ReporterId == UserContext.Id && x.BlogId == request.Id);
      }
      if (request.Entity == ReportEntity.User)
      {
        if (!await UnitOfWork.Users.AnyAsync(x => x.Id == request.Id && x.Id != UserContext.Id))
        {
          return false;
        }
        return !await UnitOfWork.Reports.AnyAsync(x => x.ReporterId == UserContext.Id && x.UserId == request.Id);
      }
      if (request.Entity == ReportEntity.Pet)
      {
        if (!await UnitOfWork.Pets.AnyAsync(x => x.Id == request.Id && x.OwnerId != UserContext.Id))
        {
          return false;
        }
        return !await UnitOfWork.Reports.AnyAsync(x => x.ReporterId == UserContext.Id && x.PetId == request.Id);
      }
      return false;
    }

    public async Task<bool> ReportAsync(ReportRequestModel request)
    {
      if (!await PreReportAsync(new PreReportRequestModel()
      {
        Id = request.Id,
        Entity = request.Entity,
      }))
      {
        return false;
      }

      if (request.Entity == ReportEntity.Blog)
        CreateReports(request, r => r.BlogId = request.Id);
      if (request.Entity == ReportEntity.User)
        CreateReports(request, r => r.UserId = request.Id);
      if (request.Entity == ReportEntity.Pet)
        CreateReports(request, r => r.PetId = request.Id);

      await UnitOfWork.SaveChangesAsync();

      var entityLabel = request.Entity switch
      {
        ReportEntity.Blog => "blog",
        ReportEntity.User => "người dùng",
        ReportEntity.Pet => "thú cưng",
        _ => "nội dung",
      };

      await _notificationService.CreateForAdminsAsync(new CreateNotificationModel
      {
        ReferenceId = request.Id,
        ReferenceType = request.Entity switch
        {
          ReportEntity.Blog => ReferenceType.Blog,
          ReportEntity.User => ReferenceType.User,
          ReportEntity.Pet => ReferenceType.Pet,
          _ => ReferenceType.Report,
        },
        Title = "Báo cáo mới",
        Content = $"Có báo cáo mới về {entityLabel}.",
        Type = NotificationType.ReportCreated,
      });

      return true;
    }

    private void CreateReports(ReportRequestModel request, Action<Report> setTarget)
    {
      foreach (var type in request.ReportTypes)
      {
        var report = new Report()
        {
          Id = Guid.NewGuid(),
          Type = type,
          ReporterId = UserContext.Id,
          IsCreatedAt = DateTimeOffset.UtcNow,
        };
        setTarget(report);
        UnitOfWork.Reports.Create(report);
      }
    }
  }
}
