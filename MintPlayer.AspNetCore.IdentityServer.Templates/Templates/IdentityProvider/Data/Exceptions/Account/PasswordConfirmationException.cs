namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;

public class PasswordConfirmationException : Exception
{
    public PasswordConfirmationException() : base("Passwords do not match")
    {
    }
}
