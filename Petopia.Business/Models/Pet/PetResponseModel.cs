#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Business.Models.Pet
{
  public class PetResponseModel
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Breed { get; set; }
    public PetSex Sex { get; set; }
    public PetAge Age { get; set; }
    public string Image { get; set; }
  }
}
