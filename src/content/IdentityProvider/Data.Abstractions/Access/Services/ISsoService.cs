using Duende.IdentityServer.Models;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;

public interface ISsoService
{
	Task<AuthorizationRequest> Login(string email, string password, string redirectUrl);
}
