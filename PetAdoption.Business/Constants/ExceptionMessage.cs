namespace PetAdoption.Business.Constants
{
  public class ExceptionMessage
  {
    // 401
    public static readonly string ACCESS_TOKEN_EXPIRED = "Access token expired";
    public static readonly string UNAUTHORIZED = "Unauthorized";
    public static readonly string INVALID_ACCESS_TOKEN = "Invalid access token";

    // 400
    public static readonly string INCORRECT_LOGIN_INFO = "Incorrect login info";
    public static readonly string DUPLICATE = "Duplicate";

    // 500
    public static readonly string UNKNOWN_ERROR = "Some unknown error occurred";
  }
}
