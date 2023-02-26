using Newtonsoft.Json;

namespace Identity.Application.Models.Parameters;

public class CompleteDataInitiatedUserParameters
{ 
    public string Code { get; set; }
    public string UserName { get; set; }  
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

    [JsonConstructor]
    public CompleteDataInitiatedUserParameters(string code, string userName, string phoneNumber,
        string password, string confirmPassword)
    {
        Code = code;
        UserName = userName; 
        PhoneNumber = phoneNumber;
        Password = password;
        ConfirmPassword = confirmPassword;
    }
}