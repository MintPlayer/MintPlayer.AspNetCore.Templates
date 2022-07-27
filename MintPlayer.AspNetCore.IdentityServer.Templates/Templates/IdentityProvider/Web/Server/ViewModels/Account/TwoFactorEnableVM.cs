using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.ViewModels.Account;

public class TwoFactorEnableVM
{
	public bool Enable { get; set; }
	public TwoFactorCode Code { get; set; }
}
