namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;

public class EmailNotConfirmedException : Exception
{
	public EmailNotConfirmedException() : base("Your email address is not confirmed")
	{
	}
}
