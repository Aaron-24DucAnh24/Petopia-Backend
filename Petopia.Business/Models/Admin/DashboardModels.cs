namespace Petopia.Business.Models.Admin
{
  public class AdminStatisticsResponseModel
  {
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int TotalOrganizations { get; set; }
    public int TotalPets { get; set; }
    public int AvailablePets { get; set; }
    public int AdoptedPets { get; set; }
    public int TotalPosts { get; set; }
    public int TotalBlogs { get; set; }
    public int TotalPayments { get; set; }
    public long TotalRevenue { get; set; }
    public int PendingReports { get; set; }
    public int ResolvedReports { get; set; }
    public List<AdminMonthlyStatsModel> MonthlyStats { get; set; } = new();
  }

  public class AdminMonthlyStatsModel
  {
    public required string Month { get; set; }
    public int Users { get; set; }
    public int Pets { get; set; }
    public int Posts { get; set; }
    public int Payments { get; set; }
  }
}
