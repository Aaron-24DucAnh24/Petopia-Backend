namespace Petopia.Business
{
  public class DomainErrorCode
  {
    // Authentication and user
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
    public const int WRONG_LOCATION_CODE = 10010;

    // Pet
    public const int NOT_FOUND_PET = 11001;

    // Adoption form
    public const int NOT_FOUND_FORM = 12001;
    public const int ADOPT_OWNED_PET = 12002;
    public const int READOPT_PET = 12003;

    // Blog
    public const int NOT_FOUND_BLOG = 13001;

    // Payment
    public const int CANNOT_CREATE_TOKEN = 14001;
    public const int CANNOT_CREATE_PAYMENT = 14002;
  }
}