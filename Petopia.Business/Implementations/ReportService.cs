using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Enums;
using Petopia.Business.Models.Report;
using Petopia.Data.Entities;

namespace Petopia.Business.Implementations
{
  public class ReportService : BaseService, IReportService
  {
    public ReportService(IServiceProvider provider, ILogger<ReportService> logger) : base(provider, logger)
    {
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
      {
        foreach (var type in request.ReportTypes)
        {
          UnitOfWork.Reports.Create(new Report()
          {
            Id = Guid.NewGuid(),
            Type = type,
            BlogId = request.Id,
            ReporterId = UserContext.Id,
          });
        }
      }
      if (request.Entity == ReportEntity.User)
      {
        foreach (var type in request.ReportTypes)
        {
          UnitOfWork.Reports.Create(new Report()
          {
            Id = Guid.NewGuid(),
            Type = type,
            UserId = request.Id,
            ReporterId = UserContext.Id,
          });
        }
      }
      if (request.Entity == ReportEntity.Pet)
      {
        foreach (var type in request.ReportTypes)
        {
          UnitOfWork.Reports.Create(new Report()
          {
            Id = Guid.NewGuid(),
            Type = type,
            PetId = request.Id,
            ReporterId = UserContext.Id,
          });
        }
      }
      await UnitOfWork.SaveChangesAsync();
      return true;
    }
  }
}