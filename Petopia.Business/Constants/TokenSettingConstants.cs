namespace Petopia.Business.Constants
{
  public class TokenSettingConstants
  {
    public const double ACCESS_TOKEN_EXPIRATION_DAYS = (double)15 / 60 / 24; // 15 mins
    public const double REFRESH_TOKEN_EXPIRATION_DAYS = (double)7;
    public const double PASSWORD_TOKEN_EXPIRATION_DAYS = (double)1 / 24; // 1 hour
  }
}
