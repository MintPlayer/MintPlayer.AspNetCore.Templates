namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;

public class RequiresTwoFactorException : Exception
{
    public Dtos.Dtos.User User { get; internal set; }
}