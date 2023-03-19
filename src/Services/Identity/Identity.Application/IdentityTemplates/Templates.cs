namespace Identity.Application.IdentityTemplates;

public class Templates
{
    public static string GetConfirmAccountTemplate()
    {
        return "<!DOCTYPE html><html><body><h2>Hi {UserName}</h2></br><p><strong>" +
               "Confirm your registration:<strong> " +
               "<a href={VerificationUri}>" +
               "confirmation</a></p></body></html>";
    }

    public static string GetResetPasswordTemplate()
    {
        return "<!DOCTYPE html><html><body><h4>Reset Password Email</h4>" +
               "<p>Please use the below token to reset your password with the <code>" +
               "{origin}/user/reset-password</code> api route: </p>" +
               "<p><code>{resetToken}</code></p></body></html>";
    }
    
    public static string GetUniqueCodeTemplate()
    {
        return "<!DOCTYPE html><html><head><title>UserCode</title>" +
               "<style>" +
               "body {" +
                   "font-family: " +
                   "Arial, sans-serif;" +
                   "margin: 0;" +
                   "padding: 0;" +
                   "background-color: #f2f2f2;" +
               "}" +
               ".container {" +
                    "max-width: 800px;" +
                    "margin: 0 auto;" +
                    "padding: 40px;" +
                    "background-color: #fff;" +
                    "box-shadow: 0 0 10px rgba(0,0,0,0.2);" +
                    "border-radius: 5px;" +
                    "text-align: center;" +
               "}" +
               "h1 {" +
                   "margin-top: 0;" +
                   "color: #333;" +
               "}" +
               "p {" +
                   "margin: 20px 0;" +
                   "color: #666;" +
               "}" +
               ".code {" +
                   "font-size: 1.2em;" +
                   "font-weight: bold;" +
                   "color: #333;" +
               "}" +
               "</style>" +
               "</head>" +
                   "<body>" + 
                        "<div class=\"container\">" +  
                           "<h1>Welcome to our company.</h1>" +
                           "<h3>Organization code: <span class=\"code\">{organizationCode}</span></h3>" +
                           "<h3>Your code is: <span class=\"code\">{uniqueCode}</span></h3>" +
                           "<h3>Please keep these codes in a safe place.</h3>" + 
                       "</div>" +  
                   "</body>" +
               "</html>";
    }
}