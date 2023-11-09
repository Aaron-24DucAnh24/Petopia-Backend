namespace Petopia.Business.Models.Authentication
{
  public class CacheRegisterRequestModel
  {
    public RegisterRequestModel Request { get; set; } = null!;
    public string RegisterToken { get; set; } = null!;
  }
}