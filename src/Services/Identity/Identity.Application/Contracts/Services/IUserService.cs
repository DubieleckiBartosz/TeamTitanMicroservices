using Identity.Application.Models.DataTransferObjects;
using Identity.Application.Wrappers;

namespace Identity.Application.Contracts.Services;

public interface IUserService
{
    Task<Response<string>> InitUserAsync(InitUserDto initUserDto);

    Task<Response<string>> CompleteDataInitiatedUser(CompleteDataInitiatedUserDto completeDataInitiatedUser,
        string origin);
    Task<Response<int>> RegisterNewUserAsync(RegisterDto registerDto, string origin);
    Task<Response<AuthenticationDto>> LoginAsync(LoginDto loginDto);
    Task<Response<string>> AddToRoleAsync(UserNewRoleDto userNewRoleDto);
    Task<Response<string>> AddToOwnerRoleAsync(UserOwnerRoleDto userOwnerRoleDto);
    Task<Response<AuthenticationDto>> RefreshTokenAsync(string refreshTokenKey);
    Task<Response<string>> RevokeTokenAsync(string tokenKey);
    Task<Response<UserCurrentIFullInfoDto>> GetCurrentUserInfoAsync(string token);
    Task<Response<string>> VerifyEmail(VerifyAccountDto verifyAccountDto);
    Task<Response<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<Response<string>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, string origin);
}