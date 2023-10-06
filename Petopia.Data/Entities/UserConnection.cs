#nullable disable

namespace Petopia.Data.Entities
{
  public class UserConnection
  {
    public string Id { get; set; }
    public string AccessToken { get; set; }
    public DateTimeOffset AccessTokenExpirationDate { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpirationDate { get; set; }
    public bool IsDeleted { get; set; }
    public User User { get; set; }
  }
}