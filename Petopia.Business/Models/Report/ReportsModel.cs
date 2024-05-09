using Petopia.Business.Models.Enums;
using Petopia.Data.Enums;

namespace Petopia.Business.Models.Report
{
  public class PreReportRequestModel
  {
    public Guid Id { get; set; }
    public ReportEntity Entity { get; set; }
  }

  public class ReportRequestModel : PreReportRequestModel
  {
    public List<ReportType> ReportTypes { get; set; } = null!;
  }
}