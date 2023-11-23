namespace Petopia.Business.Models.Common
{
  public class PaginationModel<T>
  {
    public required T Data { get; set; }
    public int ItemsNumber { get; set; }
    public int TotalNumber { get; set; }
    public int PageIndex { get; set; }
    public int PageNumber { get; set; }
  }
}