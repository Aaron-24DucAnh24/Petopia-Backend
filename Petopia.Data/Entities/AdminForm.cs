#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
  public class AdminForm
  {
    public Guid Id { get; set; }
    public string Email { get; set; }
    public AdminFormStatus Status { get; set; }
  }
}