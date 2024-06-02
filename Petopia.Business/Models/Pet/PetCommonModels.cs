using Petopia.Data.Enums;

# nullable disable

namespace Petopia.Business.Models.Pet
{
  public class CreatePetRequestModel
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public PetSex Sex { get; set; }
    public PetAge Age { get; set; }
    public PetColor Color { get; set; }
    public PetSpecies Species { get; set; }
    public PetSize Size { get; set; }
    public PetDedicalStatus IsSterillized { get; set; }
    public PetDedicalStatus IsVaccinated { get; set; }
    public bool IsAvailable { get; set; }
    public string Breed { get; set; }
    public List<string> Images { get; set; }
    public List<Guid> VaccineIds { get; set; }
  }

  public class CreatePetResponseModel : CreatePetRequestModel
  {
    public Guid Id { get; set; }
  }
  public class UpdatePetRequestModel : CreatePetResponseModel { }

  public class UpdatePetResponseModel : CreatePetResponseModel { }

  public class PetDetailsResponseModel
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public PetSex Sex { get; set; }
    public PetAge Age { get; set; }
    public PetColor Color { get; set; }
    public PetSpecies Species { get; set; }
    public PetSize Size { get; set; }
    public PetDedicalStatus IsSterillized { get; set; }
    public PetDedicalStatus IsVaccinated { get; set; }
    public bool IsAvailable { get; set; }
    public string Breed { get; set; }
    public List<string> Images { get; set; }
    public Guid OwnerId { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
    public string Address { get; set; }
    public List<PetResponseModel> SeeMore { get; set; }
    public bool IsOrgOwned { get; set; }
    public List<VaccineResponseModel> Vaccines { get; set; }
  }
}
