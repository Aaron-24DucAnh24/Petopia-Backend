namespace PetAdoption.Business.Models.Setting
{
  public class RedisCacheSettingModel
  {
    public string ConnectionString { set; get; } = null!;
    public string InstanceName { set; get; } = null!;
  }
}