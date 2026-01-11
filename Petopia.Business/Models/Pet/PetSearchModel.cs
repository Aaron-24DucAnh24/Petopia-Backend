using Petopia.Data.Enums;

namespace Petopia.Business.Models.Pet
{
  public class PetSearchModel : PetResponseModel
  {
    public PetColor Color { get; set; }
    public PetSpecies Species { get; set; }
    public PetSize Size { get; set; }
    public bool IsVaccinated { get; set; }
    public bool IsSterillized { get; set; }
    public int View { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
  }
}