using Azure.Core;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Enums;
using Petopia.Business.Models.Report;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

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
        return !await UnitOfWork.Reports.AnyAsync(x => x.ReporterId == UserContext.Id && x.BlogId == request.Id);
      }
      if (request.Entity == ReportEntity.User)
      {
        return !await UnitOfWork.Reports.AnyAsync(x => x.ReporterId == UserContext.Id && x.UserId == request.Id);
      }
      if (request.Entity == ReportEntity.Pet)
      {
        return !await UnitOfWork.Reports.AnyAsync(x => x.ReporterId == UserContext.Id && x.PetId == request.Id);
      }
      return false;
    }

    public async Task<bool> ReportAsync(ReportRequestModel request)
    {
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