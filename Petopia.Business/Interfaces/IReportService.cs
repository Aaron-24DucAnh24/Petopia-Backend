using Petopia.Business.Models.Report;

namespace Petopia.Business.Interfaces
{
  public interface IReportService
  {
    public Task<bool> PreReportAsync(PreReportRequestModel request);
    public Task<bool> ReportAsync(ReportRequestModel request);
  }
}