namespace Identity.Application.IdentityTemplates;

public class TemplateCreator
{
    public static Dictionary<string, string> TemplateRegisterAccount(string userName, string code)
    {
        var dictData = new Dictionary<string, string>
        {
            {"UserName", userName},
            {"VerificationUri", code}
        };
        return dictData;
    }

    public static Dictionary<string, string> TemplateResetPassword(string resetToken, string origin)
    {
        var dictData = new Dictionary<string, string>
        {
            {"resetToken", resetToken},
            {"origin", origin}
        };
        return dictData;
    }
    
    public static Dictionary<string, string> TemplateInitUser(string uniqueCode, string organizationCode)
    {
        var dictData = new Dictionary<string, string>
        {
            {"uniqueCode", uniqueCode},
            {"organizationCode", organizationCode}
        };
        return dictData;
    }
}