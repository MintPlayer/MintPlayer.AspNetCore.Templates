namespace MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

public class LoginResult
{
	public ELoginStatus Status { get; set; }
	public User User { get; set; }
}