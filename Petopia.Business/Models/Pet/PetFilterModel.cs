using Petopia.Data.Enums;

# nullable disable

namespace Petopia.Business.Models.Pet
{
  public class PetFilterModel
  {
    public PetSex Sex { get; set; }
    public PetColor Color { get; set; }
    public PetSpecies Species { get; set; }
    public PetSize Size { get; set; }
    public string Breed { get; set; }
    public bool IsVaccinated { get; set; }
  }
}
