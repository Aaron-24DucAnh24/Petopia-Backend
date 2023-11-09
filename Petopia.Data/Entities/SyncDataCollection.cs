#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
  public class SyncDataCollection
  {
    public Guid CollectionId { set; get; }
    public required Guid ItemId { set; get; }
    public string Index { set; get; }
    public SyncDataStatus Status { get; set; }
    public SyncDataAction Action { get; set; }
  }
}