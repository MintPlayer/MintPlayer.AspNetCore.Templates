namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Services;

public interface IDatabaseService
{
    void Migrate();
}