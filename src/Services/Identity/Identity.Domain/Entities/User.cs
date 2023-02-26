using Identity.Domain.EnumerationClasses;
using Identity.Domain.Exceptions;
using Identity.Domain.ValueObjects;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;

namespace Identity.Domain.Entities;

public class User : Entity, IAggregateRoot
{  
    public string UserName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public bool IsConfirmed { get; private set; }
    public string PasswordHash { get; private set; } 
    public string? VerificationCode { get; private set; }
    public TokenValue VerificationToken { get; private set; }
    public TokenValue? ResetToken { get; private set; }
    public List<RefreshToken> RefreshTokens { get; private set; }
    public List<Role> Roles { get; private set; }

    /// <summary>
    /// Create employee or manager by manager or owner
    /// </summary>
    /// <param name="verificationCode"></param>
    /// <param name="role"></param>
    /// <param name="email"></param>
    private User(string verificationCode, int role, string email)
    { 
        IsConfirmed = false;  
        VerificationCode = verificationCode;
        Email = email;
        var userRole = Enumeration.GetById<Role>(role);
        Roles = new List<Role> { userRole };
    }

    /// <summary>
    /// Load data for user registration
    /// </summary>
    /// <param name="id"></param> 
    /// <param name="verificationCode"></param>
    /// <param name="role"></param>
    /// <param name="email"></param>
    private User(int id, string verificationCode, int role, string email)
    {
        Id = id;
        Email = email;
        VerificationCode = verificationCode;
        var userRole = Enumeration.GetById<Role>(role);
        Roles = new List<Role> { userRole };
    }

    /// <summary>
    /// Create new user
    /// </summary>
    /// <param name="verificationToken"></param>
    /// <param name="userName"></param>
    /// <param name="email"></param>
    /// <param name="phoneNumber"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private User(TokenValue verificationToken, string userName, string email, string phoneNumber)
    { 
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        VerificationToken = verificationToken;
        IsConfirmed = false;
        ResetToken = null;
        VerificationCode = null;
        RefreshTokens = new List<RefreshToken>();
        Roles = new List<Role> { Role.User };
    }

    /// <summary>
    /// Load user for user
    /// </summary>
    /// <param name="id"></param> 
    /// <param name="verificationCode"></param>
    /// <param name="isConfirmed"></param>
    /// <param name="resetToken"></param>
    /// <param name="resetTokenExpirationDate"></param>
    /// <param name="verificationToken"></param>
    /// <param name="verificationTokenExpirationDate"></param> 
    /// <param name="userName"></param>
    /// <param name="email"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="passwordHash"></param>
    /// <param name="roles"></param>
    /// <param name="refreshTokens"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private User(int id, string? verificationCode, bool isConfirmed, string resetToken,
        DateTime? resetTokenExpirationDate,
        string verificationToken, DateTime? verificationTokenExpirationDate,  
        string userName,
        string email, string phoneNumber,
        string passwordHash, List<Role> roles, List<RefreshToken>? refreshTokens = null) : this(
        TokenValue.Load(verificationToken, verificationTokenExpirationDate), userName, email,
        phoneNumber)
    {
        Id = id;
        VerificationCode = verificationCode;

        if (refreshTokens != null && refreshTokens.Any())
        {
            RefreshTokens = refreshTokens;
        }

        ResetToken = TokenValue.Load(resetToken, resetTokenExpirationDate);
        IsConfirmed = isConfirmed;
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        Roles = roles ?? throw new ArgumentNullException(nameof(roles));
    }

    public static User LoadUser(int id, string verificationExternalCode, int role, string email)
    {
        return new User(id, verificationExternalCode, role, email);
    }

    public static User LoadUser(int id, string? verificationExternalCode, bool isConfirmed,
        string resetToken, DateTime? resetTokenExpirationDate,
        string verificationToken, DateTime? verificationTokenExpirationDate,  
        string userName, string email,
        string phoneNumber,
        string passwordHash, List<Role> roles, List<RefreshToken>? refreshTokens = null)
    {
        return new User(id, verificationExternalCode, isConfirmed, resetToken, resetTokenExpirationDate,
            verificationToken,
            verificationTokenExpirationDate, userName, email,
            phoneNumber, passwordHash, roles, refreshTokens);
    }


    public static User InitUser(string verificationExternalCode, int role, string email)
    {
        return new User(verificationExternalCode, role, email);
    }

    public static User CreateUser(string verificationToken, string userName, string email, string phoneNumber)
    {
        var token = TokenValue.CreateVerificationToken(verificationToken);
        return new User(token, userName, email, phoneNumber);
    }


    public void CompleteData(string verificationToken, string userName, string phoneNumber)
    {
        var token = TokenValue.CreateVerificationToken(verificationToken); 
        UserName = userName ?? throw new ArgumentNullException(nameof(userName)); 
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        VerificationToken = token;
        IsConfirmed = false;
        ResetToken = null; 
        RefreshTokens = new List<RefreshToken>();
    }


    public void SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
    public void ChangePasswordHash(string passwordHash)
    {
        SetPasswordHash(passwordHash);
    }
    public void ConfirmAccount()
    {
        IsConfirmed = true;
    }

    public void AddNewRole(Role role)
    {
        var hasThatRole = Roles.Any(_ => _ == role);
        if (hasThatRole)
        {
            throw new BusinessException(BusinessRuleErrorMessages.UniqueUserRoleErrorMessage,
                BusinessExceptionTitles.UniqueRoleTitle);
        }

        Roles.Add(role);
    }

    public void MarkAsOwner()
    {
        if (VerificationCode != null)
        {
            throw new BusinessException(BusinessRuleErrorMessages.UniqueUserRoleErrorMessage,
                BusinessExceptionTitles.UniqueRoleTitle);
        }
         
        this.AddNewRole(Role.Owner);
    }

    public void ClearResetToken()
    {
        ResetToken = null;
    }
    public void SetResetToken(string resetTokenCode)
    {
        ResetToken = TokenValue.CreateResetToken(resetTokenCode);
    }

    public RefreshToken? CurrentlyActiveRefreshToken()
    {
        var activeRefreshToken = RefreshTokens?.FirstOrDefault(_ => _.IsActive);
        return activeRefreshToken;
    }

    public RefreshToken AddNewRefreshToken(string newRefreshToken)
    {
        var activeRefreshToken = CurrentlyActiveRefreshToken();
        if (activeRefreshToken != null)
        {
            throw new BusinessException(BusinessRuleErrorMessages.OnlyOneActiveRefreshTokenErrorMessage,
                BusinessExceptionTitles.OnlyOneActiveRefreshTokenTitle);
        }

        var refreshToken = RefreshToken.CreateToken(newRefreshToken);
        RefreshTokens.Add(refreshToken);

        return refreshToken;
    }

    public RefreshToken? FindToken(string tokenKey)
    {
        var result = RefreshTokens.SingleOrDefault(_ => _.TokenValue?.Token == tokenKey);
        return result;
    }

    public void RevokeToken(string refreshTokenKey)
    {
        var result = RefreshTokens.SingleOrDefault(_ => _.TokenValue?.Token == refreshTokenKey);
        if (result == null)
        {
            throw new BusinessException(BusinessRuleErrorMessages.TokenNotFoundErrorMessage,
                BusinessExceptionTitles.TokenNotFoundTitle);
        }

        result.RevokeToken();
    }
}