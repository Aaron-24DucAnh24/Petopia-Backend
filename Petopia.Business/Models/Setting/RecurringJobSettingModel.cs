namespace Petopia.Business.Models.Setting
{
  public class RecurringJobSettingModel
  {
    public string Id { get; set; } = null!;
    public string Cron { get; set; } = null!;
  }
}