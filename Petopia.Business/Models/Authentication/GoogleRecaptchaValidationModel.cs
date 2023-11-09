using System.Text.Json.Serialization;

namespace Petopia.Business.Models.Authentication
{
  public class GoogleRecaptchaValidationModel
  {
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("challenge_ts")]
    public DateTime ChallengeTimestamp { get; set; }

    [JsonPropertyName("hostname")]
    public string Hostname { get; set; } = null!;

    [JsonPropertyName("error-codes")]
    public string[]? ErrorCodes { get; set; }
  }
}