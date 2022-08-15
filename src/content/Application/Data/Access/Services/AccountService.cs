using Microsoft.AspNetCore.Authentication;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Application.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Access.Services;

internal class AccountService : IAccountService
{
	#region Constructor
	private readonly IAccountRepository accountRepository;
	public AccountService(IAccountRepository accountRepository)
	{
		this.accountRepository = accountRepository;
	}
	#endregion

	public async Task<User> GetCurrentUser()
	{
		var user = await accountRepository.GetCurrentUser();
		return user;
	}

	public async Task<IEnumerable<AuthenticationScheme>> GetExternalLoginProviders()
	{
		var providers = await accountRepository.GetExternalLoginProviders();
		return providers;
	}

	public async Task<AuthenticationProperties> ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
	{
		var properties = await accountRepository.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
		return properties;
	}

	public async Task<ExternalLoginResult> PerformExternalLogin()
	{
		var loginResult = await accountRepository.PerformExternalLogin();
		return loginResult;
	}

	public async Task Logout()
	{
		await accountRepository.Logout();
	}
}
