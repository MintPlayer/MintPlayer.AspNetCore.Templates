namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;

public interface IDatabaseService
{
    void Migrate();
}