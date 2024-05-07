namespace Petopia.Business.Models.Setting
{
  public class RecurringJobSettingModel
  {
    public string OrgJobId { get; set; } = null!;
    public string AdminJobId { get; set; } = null!;
    public string Cron { get; set; } = null!;
  }
}