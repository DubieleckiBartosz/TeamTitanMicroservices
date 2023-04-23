using Dapper;
using Identity.Application.Contracts;
using Identity.Domain.Entities;
using Microsoft.Extensions.Options;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;
using System.Data;
using System.Net;
using Identity.Application.Common.Exceptions;
using Identity.Application.Constants;
using Identity.Application.Settings;
using Identity.Infrastructure.AccessObjects;
using Identity.Infrastructure.MapDatabase;

namespace Identity.Infrastructure.Repositories;

public class UserRepository : BaseRepository<UserRepository>, IUserRepository
{
    public UserRepository(IOptions<ConnectionStrings> connectionString, ILoggerManager<UserRepository> loggerManager) :
        base(connectionString.Value.DefaultConnection, loggerManager)
    {
    }

    public async Task<User> FindByCodeAsync(string code)
    {
        var param = new DynamicParameters();

        param.Add("@uniqueCode", code);

        var result = (await this.QueryAsync<UserLessDao>("user_getUserByCode_S", param, CommandType.StoredProcedure))
            .FirstOrDefault();

        if (result == null)
        {
            throw new IdentityResultException(ExceptionIdentityMessages.UserNotFound,
                ExceptionIdentityTitles.UserByUniqueCode, HttpStatusCode.NotFound, null);
        }

        return result.Map();
    }
    public async Task<User> FindByIdAsync(int id)
    {
        var param = new DynamicParameters();

        param.Add("@userId", id);

        var result = (await this.QueryAsync<UserLessDao>("user_getUserLessById_S", param, CommandType.StoredProcedure))
            .FirstOrDefault();

        if (result == null)
        {
            throw new IdentityResultException(ExceptionIdentityMessages.UserNotFound,
                ExceptionIdentityTitles.UserByUniqueCode, HttpStatusCode.NotFound, null);
        }

        return result.Map();
    }

    public async Task<bool> CodeIsInUseAsync(string code)
    {
        var param = new DynamicParameters();

        param.Add("@code", code);

        var result = (await this.QueryAsync<bool>("user_codeIsInUse_S", param, CommandType.StoredProcedure))
            .FirstOrDefault();

        return result;
    }

    public async Task<bool> CodeExistsAsync(string code)
    {
        var param = new DynamicParameters();

        param.Add("@code", code);

        var result = (await this.QueryAsync<bool>("user_codeExists_S", param, CommandType.StoredProcedure))
            .FirstOrDefault();

        return result;
    }

    public async Task MergeCodesAsync(User user)
    {
        var param = new DynamicParameters();
         
        param.Add("@verificationCode", user.VerificationCode); 
        param.Add("@organizationCode", user.OrganizationCode); 
        param.Add("@userId", user.Id);  

        var result = await this.ExecuteAsync(sql: "user_mergeCodes_U", param: param, commandType: CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new IdentityResultException(ExceptionIdentityMessages.MergeCodesFailed,
                ExceptionIdentityTitles.MergeFailed, HttpStatusCode.InternalServerError, null);
        }
    }
    
    public async Task ClearUserCodesAsync(User user, string oldVerificationCode)
    {
        var param = new DynamicParameters();
         
        param.Add("@verificationCode", oldVerificationCode);  
        param.Add("@userId", user.Id);  

        await this.ExecuteAsync(sql: "user_clearUserCodes_UD", param: param, commandType: CommandType.StoredProcedure); 
    }

    public async Task ConfirmAccountAsync(User user)
    {
        var param = new DynamicParameters();

        param.Add("@userId", user.Id);
        param.Add("@isConfirmed", user.IsConfirmed);

        var result =
            await this.ExecuteAsync("user_confirmAccount_U", param,
                commandType: CommandType.StoredProcedure);

        if (result <= 0)
        {
            throw new IdentityResultException(ExceptionIdentityMessages.AccountApprovalFailed,
                ExceptionIdentityTitles.General, HttpStatusCode.InternalServerError, null);
        }
    }

    public async Task ClearResetTokenAsync(User user)
    {
        var param = new DynamicParameters();

        param.Add("@userId", user.Id);

        var result =
            await this.ExecuteAsync("user_clearResetToken_U", param,
                commandType: CommandType.StoredProcedure);

        if (result <= 0)
        {
            throw new IdentityResultException(ExceptionIdentityMessages.ClearResetTokenFailed,
                ExceptionIdentityTitles.ClearResetToken, HttpStatusCode.InternalServerError, null);
        }
    }

    public async Task AddToRoleAsync(User user)
    {
        var param = new DynamicParameters();

        var roleId = user.Roles.LastOrDefault()?.Id;
        param.Add("@userId", user.Id); 
        param.Add("@role", roleId);

        var result =
            await this.ExecuteAsync("user_addToRole_I", param,
                commandType: CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new IdentityResultException(ExceptionIdentityMessages.NewRoleForUserFailed,
                ExceptionIdentityTitles.NewUserRole, HttpStatusCode.InternalServerError, null);
        }
    }
     
    public async Task AddToOwnerAsync(User user)
    {
        var param = new DynamicParameters();

        var roleId = user.Roles.LastOrDefault()?.Id;
        param.Add("@userId", user.Id);
        param.Add("@role", roleId);
        param.Add("@verificationCode", user.VerificationCode);
        param.Add("@organizationCode", user.OrganizationCode);

        var result =
            await this.ExecuteAsync("user_addToOwnerRole_U", param,
                commandType: CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new IdentityResultException(ExceptionIdentityMessages.NewRoleForUserFailed,
                ExceptionIdentityTitles.NewUserRole, HttpStatusCode.InternalServerError, null);
        }
    }

    public async Task<int> CreateAsync(User user)
    {
        var param = new DynamicParameters();

        param.Add("@roleId", user.Roles.FirstOrDefault()?.Id);
        param.Add("@verificationToken", user.VerificationToken.Token);
        param.Add("@verificationTokenExpirationDate", user.VerificationToken.TokenExpirationDate);
        param.Add("@isConfirmed", user.IsConfirmed); 
        param.Add("@userName", user.UserName);
        param.Add("@email", user.Email);
        param.Add("@phoneNumber", user.PhoneNumber);
        param.Add("@passwordHash", user.PasswordHash);
        param.Add("@newIdentity", -1, DbType.Int32, ParameterDirection.Output);

        await ExecuteAsync("user_createNewUser_I", param,
            commandType: CommandType.StoredProcedure);
        var result = param.Get<int?>("@newIdentity");

        if (!result.HasValue || result <= 0)
        {
            throw new IdentityResultException(ExceptionIdentityMessages.RegisterFailed,
                ExceptionIdentityTitles.CreatingUser, HttpStatusCode.InternalServerError, null);
        }

        return result.Value;
    }
    
    public async Task CreateTemporaryUserAsync(int roleId, string verificationCode, string organizationCode)
    {
        var param = new DynamicParameters();

        param.Add("@roleId", roleId);
        param.Add("@verificationCode", verificationCode); 
        param.Add("@organizationCode", organizationCode); 

       var result = await ExecuteAsync("[dbo].[temp_createUserCodes_I]", param,
            commandType: CommandType.StoredProcedure);

       if (result <= 0)
       {
           throw new IdentityResultException(ExceptionIdentityMessages.InitCodesFailed,
               ExceptionIdentityTitles.CreatingUserCodes, HttpStatusCode.InternalServerError, null);
        }
    }

    public async Task<User> FindUserByVerificationTokenAsync(string tokenKey)
    {
        var param = new DynamicParameters();

        param.Add("@tokenKey", tokenKey);
        var dict = new Dictionary<int, UserDao?>();
        var result = (await QueryAsync<UserDao, TokenDao, int, UserDao?>("user_getUserByVerificationToken_S",
            (u, rt, r) => MapperUserDatabase.Map(dict, u, rt, r),
            param: param, splitOn: "Id,Id,RoleId", commandType: CommandType.StoredProcedure)).FirstOrDefault();

        if (result == null)
        {
            throw new IdentityResultException(ExceptionIdentityMessages.VerificationFailed,
                ExceptionIdentityTitles.AccountConfirmation, HttpStatusCode.BadRequest, null);
        }

        return result.Map();
    }

    public async Task<User> FindUserByResetTokenAsync(string tokenKey)
    {
        var param = new DynamicParameters();

        param.Add("@tokenKey", tokenKey);
        var dict = new Dictionary<int, UserDao?>();
        var result = (await QueryAsync<UserDao, TokenDao, int, UserDao?>("user_getUserByResetToken_S",
            (u, rt, r) => MapperUserDatabase.Map(dict, u, rt, r),
            param: param, splitOn: "Id,Id,RoleId", commandType: CommandType.StoredProcedure)).FirstOrDefault();

        if (result == null)
        {
            throw new IdentityResultException(ExceptionIdentityMessages.ResetPwdFailed,
                ExceptionIdentityTitles.ForgotPassword, HttpStatusCode.BadRequest, null);
        }

        return result.Map();
    }

    public async Task<User> FindUserByTokenAsync(string tokenKey)
    {
        var param = new DynamicParameters();

        param.Add("@tokenKey", tokenKey);
        var dict = new Dictionary<int, UserDao?>();
        var result = (await QueryAsync<UserDao, TokenDao, int, UserDao?>("user_getUserByToken_S",
            (u, rt, r) => MapperUserDatabase.Map(dict, u, rt, r),
            param: param, splitOn: "Id,Id,RoleId", commandType: CommandType.StoredProcedure)).FirstOrDefault();

        var token = result?.RefreshTokens.FirstOrDefault(x => x.Token == tokenKey);
        if (result == null || token == null)
        {
            throw new IdentityResultException(ExceptionIdentityMessages.TokenNotMatch,
                ExceptionIdentityTitles.UserByToken, HttpStatusCode.NotFound, null);
        }

        return result.Map();
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        var param = new DynamicParameters();

        param.Add("@email", email);

        var dict = new Dictionary<int, UserDao?>();
        var result = (await QueryAsync<UserDao?, TokenDao?, int, UserDao?>("user_getUserByEmail_S",
            (u, rt, r) => MapperUserDatabase.Map(dict, u, rt, r),
            param: param, splitOn: "Id,Id,RoleId", commandType: CommandType.StoredProcedure)).FirstOrDefault();

        return result?.Map();
    }

    public async Task UpdateAsync(User user)
    {
        var toTableType = user.RefreshTokens.Select(_ => _).ToList();

        var tokenTable = new DataTable();
        tokenTable.Columns.Add(new DataColumn("Token", typeof(string)));
        tokenTable.Columns.Add(new DataColumn("TokenExpirationDate", typeof(DateTime)));
        tokenTable.Columns.Add(new DataColumn("Created", typeof(DateTime)));
        tokenTable.Columns.Add(new DataColumn("ReplacedByToken", typeof(string)));
        tokenTable.Columns.Add(new DataColumn("Revoked", typeof(string)));

        foreach (var token in toTableType)
        {
            tokenTable.Rows.Add(token.GetTokenValue(),
                token.TokenValue.TokenExpirationDate, token.Created,
                token.ReplacedByToken,
                token.TokenActivity?.Revoked);
        }


        var param = new DynamicParameters();

        param.Add("@email", user.Email);
        param.Add("@phoneNumber", user.PhoneNumber);
        param.Add("@userId", user.Id);
        param.Add("@password", user.PasswordHash);
        param.Add("@resetToken", user.ResetToken?.Token);
        param.Add("@resetTokenExpirationDate", user.ResetToken?.TokenExpirationDate);
        param.Add("@refreshTokens",
            tokenTable.AsTableValuedParameter("UserRefreshTokensTableType"));

        var result =
            await ExecuteAsync("user_updateUserData_U", param,
                CommandType.StoredProcedure);

        if (result <= 0)
        {
            throw new IdentityResultException(ExceptionIdentityMessages.UserUpdateDataFailed,
                ExceptionIdentityTitles.UpdatingUser, HttpStatusCode.InternalServerError, null);
        }
    }

    public async Task<List<string>> GetUserRolesAsync(User user)
    {
        var param = new DynamicParameters();

        param.Add("@userId", user.Id);

        var result =
            await this.QueryAsync<string>("user_getUserRoles_S", param,
                commandType: CommandType.StoredProcedure);

        if (result == null)
        {
            throw new IdentityResultException(ExceptionIdentityMessages.UserRolesNotFound,
                ExceptionIdentityTitles.GettingRoles, HttpStatusCode.NotFound, null);
        }

        return result.ToList();
    }
     
    public async Task ClearTokens()
    {
        await this.ExecuteAsync("user_clearRevokedTokens_D",
            commandType: CommandType.StoredProcedure);
    }
}

