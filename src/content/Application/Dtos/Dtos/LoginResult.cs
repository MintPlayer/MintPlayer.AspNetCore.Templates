using MintPlayer.AspNetCore.IdentityServer.Application.Dtos.Enums;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Dtos.Dtos;

public class LoginResult
{
	public ELoginStatus Status { get; set; }
	public User User { get; set; }
	public string Error { get; set; }
	public string ErrorDescription { get; set; }
}
