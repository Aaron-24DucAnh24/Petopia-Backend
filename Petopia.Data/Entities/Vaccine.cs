#nullable disable

namespace Petopia.Data.Entities
{
  public class Vaccine
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<PetVaccine> PetVaccines { get; set; }
  }
}