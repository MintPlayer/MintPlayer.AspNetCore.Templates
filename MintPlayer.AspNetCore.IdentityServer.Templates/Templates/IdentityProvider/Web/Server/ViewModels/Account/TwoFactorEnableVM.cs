namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.ViewModels.Account;

public class TwoFactorEnableVM
{
    public bool Enable { get; set; }
    public string VerificationCode { get; set; }
}
