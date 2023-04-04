using Identity.Application.Constants;
using Identity.Application.Contracts.Services;
using Identity.Application.Models.DataTransferObjects;
using Identity.Application.Models.Parameters;
using Identity.Application.Utils;
using Identity.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Identity.API.Controllers;
  
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    } 

    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 500)]
    [ProducesResponseType(typeof(Response<string>), 200)]
    [SwaggerOperation(Summary = "Add user to owner role")]
    [HttpGet("new-owner")]
    public async Task<IActionResult> AddOwnerRoleToUser([FromQuery] UserOwnerRoleParameters parameters)
    {
        var result = await this._userService.AddToOwnerRoleAsync(new UserOwnerRoleDto(parameters));
        return Ok(result);
    }


    [Authorize]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 500)]
    [ProducesResponseType(typeof(Response<string>), 200)]
    [SwaggerOperation(Summary = "Assign codes to user")]
    [HttpPut("[action]")]
    public async Task<IActionResult> AssignUserCodes([FromBody] AssignUserCodesParameters codesParameters)
    { 
        var response = await _userService.MergeUserCodesAsync(new AssignUserCodesDto(codesParameters));
        return Ok(response);
    }

    [Authorize(Roles = "Admin,Owner,Manager,Employee")]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 500)]
    [ProducesResponseType(typeof(Response<string>), 200)]
    [SwaggerOperation(Summary = "Getting rid of the codes")]
    [HttpPut("[action]")]
    public async Task<IActionResult> ClearUserCodes()
    { 
        var response = await _userService.ClearUserCodesAsync();
        return Ok(response);
    }

    [Authorize(Roles = "Admin,Owner,Manager")]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 500)]
    [ProducesResponseType(typeof(Response<string>), 200)]
    [SwaggerOperation(Summary = "Init user organization")]
    [HttpPost("[action]")]
    public async Task<IActionResult> InitUserOrganization([FromBody] InitUserOrganizationParameters parameters)
    {
        var response = await _userService.InitUserOrganizationAsync(new InitUserOrganizationDto(parameters));
        return Ok(response);
    }

    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 500)]
    [ProducesResponseType(typeof(Response<int>), 200)]
    [SwaggerOperation(Summary = "Register new user")]
    [HttpPost("[action]")]
    public async Task<IActionResult> RegisterNewUser([FromBody] RegisterParameters parameters)
    {
        var response = await _userService.RegisterNewUserAsync(new RegisterDto(parameters), Request.Headers["origin"]);
        return Ok(response);
    }

    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 500)]
    [ProducesResponseType(typeof(Response<string>), 200)]
    [HttpGet("confirm-account")]
    [SwaggerOperation(Summary = "Confirm the account after creating the account")]
    public async Task<IActionResult> ConfirmAccountByEmail([FromQuery] string code)
    {
        var result = await _userService.VerifyEmail(new VerifyAccountDto(code));
        return Ok(result);
    }


    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 500)]
    [ProducesResponseType(typeof(Response<AuthenticationDto>), 200)]
    [SwaggerOperation(Summary = "User login")]
    [HttpPost("[action]")]
    public async Task<IActionResult> LoginUser([FromBody] LoginParameters parameters)
    {
        var response = await _userService.LoginAsync(new LoginDto(parameters));
        this.SetRefreshTokenInCookie(response.Data.RefreshToken);

        return Ok(response);
    }

    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 500)]
    [ProducesResponseType(typeof(Response<AuthenticationDto>), 200)]
    [SwaggerOperation(Summary = "Refresh token")]
    [HttpPost("[action]")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies[ConstantKeys.CookieRefreshToken];
        if (refreshToken == null)
        {
            return Unauthorized();
        }

        var response = await _userService.RefreshTokenAsync(refreshToken);
        if (!string.IsNullOrEmpty(response.Data.RefreshToken))
        {
            this.SetRefreshTokenInCookie(response.Data.RefreshToken);
        }

        return Ok(response);
    }

    [ProducesResponseType(typeof(object), 500)]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(Response<string>), 200)]
    [HttpPost("[action]")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordParameters parameters)
    {
        var result =
            await _userService.ForgotPasswordAsync(new ForgotPasswordDto(parameters), Request.Headers["origin"]);
        return Ok(result);
    }

    [ProducesResponseType(typeof(object), 500)]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(Response<string>), 200)]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordParameters parameters)
    {
        var result =
            await _userService.ResetPasswordAsync(new ResetPasswordDto(parameters));
        return Ok(result);
    }

    [Authorize]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(Response<string>), 200)]
    [SwaggerOperation(Summary = "Revoke token")]
    [HttpPost("[action]")]
    public async Task<IActionResult> RevokeToken()
    {
        var refreshToken = Request.Cookies[ConstantKeys.CookieRefreshToken];
        if (refreshToken == null)
        {
            return Unauthorized();
        }

        var result = await this._userService.RevokeTokenAsync(refreshToken);
        return Ok(result);
    }

    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 500)]
    [ProducesResponseType(typeof(Response<string>), 200)]
    [SwaggerOperation(Summary = "Add user to new role")]
    [HttpPut("[action]")]
    public async Task<IActionResult> AddNewRoleToUser([FromBody] UserNewRoleParameters parameters)
    {
        var result = await this._userService.AddToRoleAsync(new UserNewRoleDto(parameters));
        return Ok(result);
    }
     
    [Authorize]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(object), 403)]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 500)]
    [ProducesResponseType(typeof(Response<UserCurrentIFullInfoDto>), 200)]
    [SwaggerOperation(Summary = "Get info about current user")]
    [HttpGet("[action]")]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        var refreshToken = Request.Cookies[ConstantKeys.CookieRefreshToken];
        if (refreshToken == null)
        {
            return Unauthorized();
        }

        var response = await this._userService.GetCurrentUserInfoAsync(refreshToken);
        return Ok(response);
    }

    //[HttpPut("[action]")]
    //public async Task<IActionResult> UpdateUserData()
    //{
    //    return Ok("OK");
    //}

    //[HttpGet("[action]")]
    //public async Task<IActionResult> GetUsersBySearch()
    //{
    //    return Ok("OK");
    //}

    //[HttpGet("[action]")]
    //public async Task<IActionResult> GetUsersWithDetails()
    //{
    //    return Ok("OK");
    //}

    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(5),
            IsEssential = true,
            SameSite = SameSiteMode.None,
            Secure = true,
        };
        Response.Cookies.Append(ConstantKeys.CookieRefreshToken, refreshToken, cookieOptions);
    }
}