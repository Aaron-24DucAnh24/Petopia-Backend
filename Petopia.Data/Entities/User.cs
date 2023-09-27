using Petopia.Data.Enums;

#nullable disable

namespace Petopia.Data.Entities
{
  public class User
  {
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Image { get; set; }
    public UserRole Role { get; set; }
    public DateTime IsCreatedAt { get; set; }
    public string ResetPasswordToken { get; set; }
    public DateTimeOffset ResetPasswordTokenExpirationDate { get; set; }
    public UserConnection UserConnection { get; set; }
  }
}