#nullable disable

namespace Petopia.Data.Entities
{
  public class PetVaccine
  {
    public Guid PetId { get; set; }
    public Guid VaccineId { get; set; }
    public Pet Pet { get; set; }
    public Vaccine Vaccine{ get; set; }
  }
}