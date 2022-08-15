using Microsoft.AspNetCore.Authentication;
using MintPlayer.AspNetCore.IdentityServer.Application.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Repositories;

public interface IAccountRepository
{
	Task<User> GetCurrentUser();
	Task<IEnumerable<AuthenticationScheme>> GetExternalLoginProviders();
	Task<AuthenticationProperties> ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
	Task<ExternalLoginResult> PerformExternalLogin();
	Task Logout();
}
