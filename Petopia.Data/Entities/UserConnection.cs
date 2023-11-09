#nullable disable

namespace Petopia.Data.Entities
{
  public class UserConnection
  {
    public Guid Id { get; set; }
    public DateTimeOffset AccessTokenExpirationDate { get; set; }
    public DateTimeOffset RefreshTokenExpirationDate { get; set; }
    public bool IsDeleted { get; set; }
    public User User { get; set; }
  }
}