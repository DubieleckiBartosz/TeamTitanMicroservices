namespace Identity.Application.Models.DataTransferObjects;

public class UserCurrentIFullInfoDto
{ 
    public string? VerificationCode { get; private set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<string> Roles { get; set; }

    public UserCurrentIFullInfoDto(string? verificationCode, string userName,
        string email, string phoneNumber,
        List<string> roles)
    { 
        VerificationCode = verificationCode;
        UserName = userName;
        Email = email;
        PhoneNumber = phoneNumber;
        Roles = roles;
    }
}