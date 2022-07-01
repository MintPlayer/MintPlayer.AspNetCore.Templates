namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.ViewModels.Account;

public class TwoFactorBypassVM
{
    public bool Bypass { get; set; }
    public string VerificationCode { get; set; }
}