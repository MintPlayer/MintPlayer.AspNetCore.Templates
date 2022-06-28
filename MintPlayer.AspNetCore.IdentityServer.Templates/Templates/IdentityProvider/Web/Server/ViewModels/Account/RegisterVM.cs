using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.Viewmodels.Account;

public class RegisterVM
{
    public User User { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
}
