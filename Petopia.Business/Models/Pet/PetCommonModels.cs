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
    public bool IsSterillized { get; set; }
    public bool IsVaccinated { get; set; }
    public bool IsAvailable { get; set; }
    public string Address { get; set; }
    public string Breed { get; set; }
    public List<string> Images { get; set; }
  }


  public class CreatePetResponseModel : CreatePetRequestModel
  {
    public Guid Id { get; set; }
  }
  public class UpdatePetRequestModel : CreatePetResponseModel { }

  public class UpdatePetResponseModel : CreatePetResponseModel { }

  public class PetDetailsResponseModel : CreatePetResponseModel { } 
}
