﻿namespace Identity.Domain.Exceptions;

public static class BusinessRuleErrorMessages
{
    public const string CodesNullErrorMessage = "Codes are null.";
    public const string ReleaseCodesErrorMessage = "You must release codes if you want to assign new.";
    public const string UniqueUserRoleErrorMessage = "User already has that role.";
    public const string OnlyOneActiveRefreshTokenErrorMessage = "A new refresh token cannot be created while another is active.";
    public const string TokenNotFoundErrorMessage = "No token found after given key.";
}