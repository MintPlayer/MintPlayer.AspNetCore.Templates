using Microsoft.AspNetCore.Authentication;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Server.ViewModels.Provider.Auth;

public class LoginVM
{
	public string ReturnUrl { get; set; }
	public User User { get; set; }
	public string Password { get; set; }
#if (UseExternalLogins)
	public IEnumerable<AuthenticationScheme> ExternalProviders { get; set; }
#endif
	public string StylesheetUrl { get; set; }
}
