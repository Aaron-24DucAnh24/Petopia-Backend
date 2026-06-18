namespace Petopia.Business
{
  public class Constants
  {
    // Storage container name
    public const string STORAGE_CONTAINER_IMAGE = "image";

    // App setting key
    public const string APP_SETTING_KEY_TOKEN = "Token";
    public const string APP_SETTING_KEY_DB_CONNECTION_STRING = "Database";
    public const string APP_SETTING_KEY_STORAGE = "Storage";
    public const string APP_SETTING_KEY_REDIS_CACHE = "RedisCache";
    public const string APP_SETTING_KEY_CORS_ORIGIN = "CorsOrigins";
    public const string APP_SETTING_KEY_EMAIL = "Email";
    public const string APP_SETTING_KEY_GG_RECAPTCHA = "GoogleRecaptcha";
    public const string APP_SETTING_KEY_UPLOAD_CONTENT = "UploadContent";
    public const string APP_SETTING_KEY_GG_AUTH = "GoogleAuth";
    public const string APP_SETTING_KEY_ELASTICSEARCH = "Elasticsearch";
    public const string APP_SETTING_KEY_BRAINTREE = "Braintree";
    public const string APP_SETTING_KEY_APP_URLS = "AppUrls";
    public const string APP_SETTING_KEY_MINIO = "Minio";
    public const string APP_SETTING_KEY_MEILI = "MeiliSearch";

    // Claim type
    public const string CLAIM_TYPE_FIRST_NAME = "First name";
    public const string CLAIM_TYPE_LAST_NAME = "Last name";
    public const string CLAIM_TYPE_ROLE = "Role";
    public const string CLAIM_TYPE_ID = "Id";
    public const string CLAIM_TYPE_EMAIL = "Email";

    // Cookies name
    public const string COOKIES_NAME_ACCESS_TOKEN = "accessToken";
    public const string COOKIES_NAME_REFRESH_TOKEN = "refreshToken";

    // Email key
    public const string EMAIL_KEY_EMAIL = "{email}";
    public const string EMAIL_KEY_FO_ROUTE = "{foRoute}";
    public const string EMAIL_KEY_REGISTER_TOKEN = "{registerToken}";
    public const string EMAIL_KEY_PASSWORD_TOKEN = "{passwordToken}";
    public const string EMAIL_KEY_PAYMENT_ID = "{paymentId}";
    public const string EMAIL_KEY_START_DATE = "{startDate}";
    public const string EMAIL_KEY_END_DATE = "{endDate}";
    public const string EMAIL_KEY_DESCRIPTION = "{description}";
    public const string EMAIL_KEY_PRICE = "{price}";
    public const string EMAIL_KEY_PASSWORD = "{password}";

    // Order key
    public const string SORT_KEY_POPULAR = "popular";
    public const string SORT_KEY_NEWEST = "newest";

    // Token setting
    public const double TOKEN_SETTING_ACCESS_TOKEN_EXPIRATION_DAYS = (double)15 / 60 / 24;
    public const double TOKEN_SETTING_REFRESH_TOKEN_EXPIRATION_DAYS = 7;
    public const double TOKEN_SETTING_PASSWORD_TOKEN_EXPIRATION_DAYS = (double)1 / 24;
    public const double TOKEN_SETTING_REGISTER_TOKEN_EXPIRATION_DAYS = (double)30 / 60 / 24;

    // Meili search index
    public const string MEILISEARCH_INDEX_PET = "pet";
    public const string MEILISEARCH_INDEX_BLOG = "blog";

    // Log configuration
    public const string LOG_PATH_INFO = "Logs/Info.log";
    public const string LOG_PATH_WARNING = "Logs/Warning.log";
    public const string LOG_PATH_ERROR = "Logs/Error.log";
    public const string LOG_OUTPUT_TEMPLATE = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}";

    // Cache key
    public const string CACHE_KEY_PROVINCES = "provinces";

    // Cache time
    public const double CACHE_TIME_LOCATION = (double)30 / 60 / 24;
  }
}