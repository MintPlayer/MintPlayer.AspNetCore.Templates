using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Services;

internal class AccountService : IAccountService
{
    #region Constructor
    private readonly IAccountRepository accountRepository;
    public AccountService(IAccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }
    #endregion
}
