#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
  public class AdoptionForm
  {
    public Guid Id { get; set; }
    public Guid PetId { get; set; }
    public Guid AdopterId { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
    public DateTimeOffset IsUpdatedAt { get; set; }
    public AdoptStatus Status { get; set; }
    public AdoptHouseType HouseType { get; set; }
    public AdoptDelayDuration DelayDuration { get; set; }
    public string Message { get; set; }
    public bool IsSeen { get; set; }
    public bool IsSeenByAdmin { get; set; }
    public string Name { get; set; }
    public bool IsOwnerBefore { get; set; }

    public Pet Pet { get; set; }
    public User Adopter { get; set; }
  }
}