namespace Petopia.Business.Models.Setting
{
  public class ElasticsearchSettingModel
  {
    public string Password {get; set;} = null!;
    public string Username {get; set;} = null!;
    public string Url {get; set;} = null!;
  }
}