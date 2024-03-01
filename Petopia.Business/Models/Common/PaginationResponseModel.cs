namespace Petopia.Business.Models.Common
{
  public class PaginationResponseModel<T>
  {
    public List<T> Data { get; set; } = null!;
    public int PageSize { get; set; }
    public int TotalNumber { get; set; }
    public int PageIndex { get; set; }
    public int PageNumber { get; set; }
  }
}