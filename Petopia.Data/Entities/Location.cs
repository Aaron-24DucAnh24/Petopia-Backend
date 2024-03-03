#nullable disable

namespace Petopia.Data.Entities
{
  public class Province
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
  }

  public class District
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string ParentCode { get; set; }
  }

  public class Ward
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string ParentCode { get; set; }
  }
}