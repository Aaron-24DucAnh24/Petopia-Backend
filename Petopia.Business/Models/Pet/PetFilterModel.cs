using Petopia.Data.Enums;

namespace Petopia.Business.Models.Pet
{
  public class PetFilterModel
  {
    public List<PetSex>? Sex { get; set; }
    public List<PetColor>? Color { get; set; }
    public List<PetSpecies>? Species { get; set; }
    public List<PetSize>? Size { get; set; }
    public List<PetAge>? Age { get; set; }
    public List<PetDedicalStatus>? IsVaccinated { get; set; }
    public List<PetDedicalStatus>? IsSterillized { get; set; }
    public List<string>? Breed { get; set; }
    public string Text { get; set; } = null!;
  }
}
