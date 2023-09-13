namespace PetAdoption.Business.Interfaces
{
  public interface ICacheManager
  {
    public ICacheProvider Instance { get; }
  }
}