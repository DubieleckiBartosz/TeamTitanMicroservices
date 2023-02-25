﻿using Identity.Domain.Entities;

namespace Identity.Application.Contracts;

public interface IUserRepository
{
    Task CompleteDataAsync(User user);
    Task<User> FindByCodeAsync(string code);
    Task InitUserAsync(User user);
    Task<bool> CodeIsInUseAsync(string code);
    Task<int> CreateAsync(User user);
    Task ConfirmAccountAsync(User user);
    Task<User> FindUserByVerificationTokenAsync(string tokenKey);
    Task<User> FindUserByResetTokenAsync(string tokenKey);
    Task<User> FindUserByTokenAsync(string tokenKey);
    Task<User?> FindByEmailAsync(string email);
    Task UpdateAsync(User user);
    Task AddToRoleAsync(User user);
    Task<List<string>> GetUserRolesAsync(User user);
    Task ClearResetTokenAsync(User user);
    Task ClearTokens();
}