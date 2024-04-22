#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
	public class PetBreed
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Code { get; set; }
		public PetSpecies Species { get; set; }
	}
}