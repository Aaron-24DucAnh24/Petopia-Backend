namespace PetAdoption.Business.Constants
{
  public class ExceptionMessage
  {
    // 401
    public const string ACCESS_TOKEN_EXPIRED = "Access token expired";
    public const string UNAUTHORIZED = "Unauthorized";
    public const string INVALID_ACCESS_TOKEN = "Invalid access token";

    // 400
    public const string INCORRECT_LOGIN_INFO = "Incorrect login info";
    public const string DUPLICATE = "Duplicate";

    // 500
    public const string UNKNOWN_ERROR = "Some unknown error occurred";
  }
}
