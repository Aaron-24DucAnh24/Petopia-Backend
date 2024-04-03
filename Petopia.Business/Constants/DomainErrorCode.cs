namespace Petopia.Business.Constants
{
  public class DomainErrorCode
  {
    // AUTHENTICATION AND USER
    public const int INCORRECT_MAIL = 10000;
    public const int INCORRECT_CREDENTIAL = 10001;
    public const int USED_EMAIL = 10002;
    public const int INVALID_REGISTER_TOKEN = 10003;
    public const int INVALID_PASSWORD_TOKEN = 10004;
    public const int EXPIRED_GOOGLE_RECAPTCHA_TOKEN = 10005;
    public const int WRONG_LOGIN_TYPE = 10006;
    public const int INVALID_PASSWORD = 10007;
    public const int NOT_FOUND_USER = 10008;
    public const int INCORRECT_PASSWORD = 10009;

		// PET
		public const int NOT_FOUND_PET = 11007;
  }
}