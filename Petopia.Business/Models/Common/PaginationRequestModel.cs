namespace Petopia.Business.Models.Common
{
  public class PaginationRequestModel<T>
  {
    public int PageIndex { get; set; } = 1;
    public int? PageSize { get; set; }
    public required T Filter { get; set; }
    public string? OrderBy { get; set; }
  }
}
