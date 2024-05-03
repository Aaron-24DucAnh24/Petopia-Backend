namespace Petopia.Business.Models.User
{
  public class UpdateUserRequestModel
  {
    public string Phone { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string ProvinceCode { get; set; } = null!;
    public string DistrictCode { get; set; } = null!;
    public string WardCode { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string Website { get; set; } = null!;
    public string Description { get; set; } = null!;
  }
}

