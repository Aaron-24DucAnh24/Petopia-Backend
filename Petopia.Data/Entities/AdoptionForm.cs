#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
    public class AdoptionForm
    {
        public Guid AdoptionFormId { get; set; }
        public Guid PetId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        //Display

        public string FName { get; set; }
        public string LName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }

        public string Adr { get; set; }
        public string AdrCity { get; set; }
        public string AdrDistrict { get; set; }

        public bool IsPetOwner { get; set; }
        public HouseType HouseType { get; set; }
        public TakePetDuration TakePetDuration { get; set; }

        /*public DateTime UpdatedAt { get; set; }*/
    }
}