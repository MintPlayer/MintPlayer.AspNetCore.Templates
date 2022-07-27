namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;

public class LoginException : Exception
{
	public Guid UserId { get; set; }
	public string Email { get; set; }
	public string Username { get; set; }
}
