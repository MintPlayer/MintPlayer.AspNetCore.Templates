namespace MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

public class RegisterResult
{
	public bool RequiresEmailConfirmation { get; set; }
	public User? User { get; set; }
}
