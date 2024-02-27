using Petopia.Data.Enums;

namespace Petopia.Business.Models.AdoptionForm
{
    public class AdoptionFormDataModel
    {
        public Guid AdoptionFormId { get; set; }

        //Display
        public string FName { get; set; } = string.Empty;

        public string LName { get; set; } = string.Empty;

        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNum { get; set; } = string.Empty;

        public string Adr { get; set; } = string.Empty;
        public string AdrCity { get; set; } = string.Empty;
        public string AdrDistrict { get; set; } = string.Empty;

        public bool IsPetOwner { get; set; }
        public HouseType HouseType { get; set; }
        public TakePetDuration TakePetDuration { get; set; }
    }
}