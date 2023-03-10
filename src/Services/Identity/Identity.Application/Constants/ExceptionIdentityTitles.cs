namespace Identity.Application.Constants;

public class ExceptionIdentityTitles
{
    public const string CreatingUser = "Creating a user.";
    public const string UserByToken = "Searching for a user with a token.";
    public const string UserByEmail = "Searching for a user with a mail.";
    public const string GettingRoles = "Getting roles.";
    public const string UserByUniqueCode = "Code is wrong.";
    public const string UpdatingUser = "Error updating user data.";
    public const string NewUserRole = "Error adding a new role.";
    public const string UserExists = "The user cannot exist.";
    public const string RoleDoesNotExist = "Role Search.";
    public const string ValidationError = "Validation error.";
    public const string General = "Something went wrong.";
    public const string AccountConfirmation = "Account confirmation.";
    public const string ForgotPassword = "New token due to forgotten password.";
    public const string ClearResetToken = "Clearing the reset token.";
    public const string ResetToken = "Token Status."; 
    public const string IncorrectCode = "Incorrect code."; 
}