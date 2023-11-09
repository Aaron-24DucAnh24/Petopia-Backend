namespace Petopia.Business.Interfaces
{
  public interface ICacheManager
  {
    public ICacheProvider Instance { get; }
  }
}