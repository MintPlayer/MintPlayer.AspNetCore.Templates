namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.ViewModels.Account;

public class TwoFactorLoginVM
{
    public string VerificationCode { get; set; }
    public bool RememberDevice { get; set; }
}
