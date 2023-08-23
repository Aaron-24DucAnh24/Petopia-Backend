namespace PetAdoption.Business.Models
{
  public class ErrorResponse<T>
  {
    public string StatusCode { get; set; } = null!;
    public required T Error { get; set; }
  }
}