﻿using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;

public interface IAccountRepository
{
    public Task<User> Register(User user, string password);
    Task<string> GenerateEmailConfirmationToken(string email);
    Task VerifyEmailConfirmationToken(string email, string token);
    Task<User> Login(string email, string password, bool createCookie);
    Task<User> TwoFactorLogin(string verificationCode, bool rememberDevice);
    Task<User> GetCurrentUser();
    Task<IEnumerable<string>> GetRoles();
    Task Logout();
    Task<bool> HasPassword();
    Task ChangePassword(string? currentPassword, string newPassword, string newPasswordConfirmation);
    Task<string> GenerateTwoFactorRegistrationCode();
    Task SetEnableTwoFactor(bool enable, string verificationCode);
    Task<int> GetRemainingRecoveryCodes();
    Task SetBypassTwoFactor(bool bypass, string verificationCode);
    Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodes(string verificationCode);
}
