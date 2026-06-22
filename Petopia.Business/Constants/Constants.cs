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

    // Google API query param keys
    public const string GOOGLE_PARAM_ACCESS_TOKEN = "access_token";
    public const string GOOGLE_PARAM_RESPONSE = "response";
    public const string GOOGLE_PARAM_SECRET = "secret";

    // Pet search
    public const int PET_SEE_MORE_LENGTH = 4;
    public const string PET_NAMES_CACHE_KEY = "NAMES_CACHE_KEY";
    public const string PET_BREEDS_CACHE_KEY = "BREEDS_CACHE_KEY";
    public const string PET_DOG_KEYWORD = "Chó";
    public const string PET_CAT_KEYWORD = "Mèo";

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
    public const string CACHE_KEY_ADMIN_STATISTICS = "admin:statistics";

    // Cache time
    public const double CACHE_TIME_LOCATION = (double)30 / 60 / 24;
    public const double CACHE_TIME_ADMIN_STATISTICS = (double)10 / 60 / 24;

    // Pagination
    public const int DEFAULT_PAGE_SIZE = 10;

    // SignalR
    public const string SIGNALR_USER_GROUP_PREFIX = "user_";
    public const string SIGNALR_EVENT_RECEIVE_NOTIFICATION = "ReceiveNotification";

    // Storage
    public const string STORAGE_URL_PROTOCOL = "http";

    // Notification messages
    public const string NOTIFICATION_TITLE_ACCOUNT_DEACTIVATED = "Tài khoản bị vô hiệu hóa";
    public const string NOTIFICATION_CONTENT_ACCOUNT_DEACTIVATED = "Tài khoản của bạn đã bị quản trị viên vô hiệu hóa.";
    public const string NOTIFICATION_TITLE_POST_DEACTIVATED = "Bài đăng bị vô hiệu hóa";
    public const string NOTIFICATION_CONTENT_POST_DEACTIVATED = "Bài đăng của bạn đã bị quản trị viên vô hiệu hóa.";
    public const string NOTIFICATION_TITLE_PET_DEACTIVATED = "Thú cưng bị vô hiệu hóa";
    public const string NOTIFICATION_TITLE_BLOG_HIDDEN = "Blog bị ẩn";
    public const string NOTIFICATION_TITLE_REPORT_RESOLVED = "Báo cáo đã được xử lý";
    public const string NOTIFICATION_CONTENT_REPORT_RESOLVED = "Báo cáo của bạn đã được quản trị viên xem xét và xử lý.";
    public const string NOTIFICATION_TITLE_UPGRADE_APPROVED = "Yêu cầu nâng cấp được chấp nhận";
    public const string NOTIFICATION_CONTENT_UPGRADE_APPROVED = "Yêu cầu nâng cấp tài khoản của bạn đã được chấp nhận. Tài khoản của bạn đã được nâng cấp thành tổ chức.";
    public const string NOTIFICATION_TITLE_UPGRADE_REJECTED = "Yêu cầu nâng cấp bị từ chối";
    public const string NOTIFICATION_CONTENT_UPGRADE_REJECTED = "Yêu cầu nâng cấp tài khoản của bạn đã bị từ chối.";
  }
}