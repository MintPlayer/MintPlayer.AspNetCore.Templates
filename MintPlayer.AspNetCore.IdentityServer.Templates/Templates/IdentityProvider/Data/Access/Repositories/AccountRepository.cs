using Microsoft.AspNetCore.Identity;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Repositories;

internal class AccountRepository : IAccountRepository
{
    #region Constructor
    private readonly UserManager<Persistance.Entities.User> userManager;
    public AccountRepository(UserManager<Persistance.Entities.User> userManager)
    {
        this.userManager = userManager;
    }
    #endregion
}
