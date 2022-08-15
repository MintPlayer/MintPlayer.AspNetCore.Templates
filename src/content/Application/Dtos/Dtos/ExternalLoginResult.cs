namespace MintPlayer.AspNetCore.IdentityServer.Application.Dtos.Dtos;

public class ExternalLoginResult : LoginResult
{
	public string? TargetOrigin { get; set; }
	public string Provider { get; set; }
}
