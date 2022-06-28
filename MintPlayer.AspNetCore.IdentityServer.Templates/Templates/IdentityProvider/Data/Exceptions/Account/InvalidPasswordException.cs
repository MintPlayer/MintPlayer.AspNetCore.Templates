namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException() : base("The password is incorrect")
    {
    }
}