namespace PetAdoption.Business.Models
{
  public class RedisCacheSettingModel
  {
    public string ConnectionString { set; get; } = null!;
    public string InstanceName { set; get; } = null!;
  }
}