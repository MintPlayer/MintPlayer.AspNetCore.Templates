namespace MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

public class ExternalLoginResult : LoginResult
{
	public string? TargetOrigin { get; set; }
	public string Provider { get; set; }
}
