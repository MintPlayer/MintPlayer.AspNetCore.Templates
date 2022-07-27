namespace MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Enums;

public enum ELoginStatus
{
	Failed,
	Success,
	RequiresTwoFactor,
	NotActivated,
	MustChangePassword,
	InternalError,
}
