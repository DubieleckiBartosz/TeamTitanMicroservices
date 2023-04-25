namespace Identity.Application.Constants;

public class ConstantKeys
{    
    //cookie
    public const string CookieRefreshToken = "cookieRefreshTokenKey";

    //Hangfire recurring jobs 
    public const string ClearTokensRecurringJob = "clearTokens";
    public const string ClearTempUsersRecurringJob = "clearTempUsers";
}