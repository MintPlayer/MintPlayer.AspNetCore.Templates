using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Services;

internal class SsoService : ISsoService
{
	#region Constructor
	private readonly IAccountRepository accountRepository;
	private readonly IHttpContextAccessor httpContextAccessor;
	private readonly IEventService events;
	private readonly IIdentityServerInteractionService interaction;
	public SsoService(
		IAccountRepository accountRepository,
		IHttpContextAccessor httpContextAccessor,
		IEventService events,
		IIdentityServerInteractionService interaction)
	{
		this.accountRepository = accountRepository;
		this.httpContextAccessor = httpContextAccessor;
		this.events = events;
		this.interaction = interaction;
	}
	#endregion

	public async Task<AuthorizationRequest> Login(string email, string password, string redirectUrl)
	{
		var request = await interaction.GetAuthorizationContextAsync(redirectUrl);
		try
		{
			var user = await accountRepository.Login(email, password, true);
			var userId = user.Id.ToString();

			await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, userId, user.UserName, clientId: request?.Client.ClientId));
			var isUser = new IdentityServerUser(userId)
			{
				DisplayName = user.UserName,
				AdditionalClaims = new[]
				{
					new Claim(ClaimTypes.NameIdentifier, userId),
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(ClaimTypes.Email, user.Email),
				}
			};
			await httpContextAccessor.HttpContext.SignInAsync(isUser);

			return request;
		}
		catch (LoginException loginEx)
		{
			await events.RaiseAsync(new UserLoginFailureEvent(loginEx.Email, "invalid credentials", clientId: request?.Client.ClientId));
			throw loginEx;
		}
	}
}
