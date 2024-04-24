#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
  public class Pet
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Breed { get; set; }
    public PetSex Sex { get; set; }
    public PetAge Age { get; set; }
    public PetColor Color { get; set; }
    public PetSpecies Species { get; set; }
    public PetSize Size { get; set; }
    public PetDedicalStatus IsVaccinated { get; set; }
    public PetDedicalStatus IsSterillized { get; set; }
    public bool IsAvailable { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
    public DateTimeOffset IsUpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public Guid OwnerId { get; set; }
    public int View { get; set; }

    public User Owner { get; set; }
    public List<Media> Images { get; set; }
    public List<AdoptionForm> AdoptionForms { get; set; }
    public List<Post> Posts { get; set; }
  }
}
