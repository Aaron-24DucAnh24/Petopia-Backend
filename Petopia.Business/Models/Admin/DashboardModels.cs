namespace Petopia.Business.Models.Admin
{
  public class DashboardReponseModel
  {
    public int Income { get; set; }
    public int PetNumber { get; set; }
    public int IndividualNumber { get; set; }
    public int OrganizationNumber { get; set; }
    public int BlogNumber { get; set; }
    public int ActiveRate { get; set; }
    public List<int> PetData { get; set; } = null!;
    public List<int> AdoptionData { get; set; } = null!;
    public List<int> BlogData { get; set; } = null!;
  }

  public class DashboardRequestModel
  {
    public int Month { get; set; }
    public int Year { get; set; }
  }
}