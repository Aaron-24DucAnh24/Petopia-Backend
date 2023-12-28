namespace Petopia.Business.Models.Common
{
  public class PaginationResponseModel<T>
  {
    public required List<T> Data { get; set; }
    public int PageSize { get; set; }
    public int TotalNumber { get; set; }
    public int PageIndex { get; set; }
    public int PageNumber { get; set; }
  }
}