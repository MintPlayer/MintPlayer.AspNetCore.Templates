namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.ViewModels.Account;

public class ChangePasswordVM
{
	public string? CurrentPassword { get; set; }
	public string NewPassword { get; set; } = string.Empty;
	public string NewPasswordConfirmation { get; set; } = string.Empty;
}
