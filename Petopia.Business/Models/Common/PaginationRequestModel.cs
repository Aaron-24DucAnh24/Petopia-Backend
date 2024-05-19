namespace Petopia.Business.Models.Common
{
  public class PaginationRequestModel<T> : PaginationRequestModel
  {
    public required T Filter { get; set; }
  }

  public class PaginationRequestModel
  {
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; }
    public string? OrderBy { get; set; }
  }
}
