namespace PetAdoption.Business.Models
{
  public class StorageSettingModel
  {
    public string ConnectionString { set; get; } = null!;
    public string MountedPath { set; get; } = null!;
  }
}