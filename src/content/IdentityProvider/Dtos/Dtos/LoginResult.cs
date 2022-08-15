using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Enums;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

public class LoginResult
{
	public ELoginStatus Status { get; set; }
	public User User { get; set; }
	public string Error { get; set; }
	public string ErrorDescription { get; set; }
}
