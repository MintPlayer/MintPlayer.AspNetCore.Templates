namespace MintPlayer.AspNetCore.IdentityServer.Application.Dtos.Enums;

public enum ELoginStatus
{
	Failed,
	Success,
	RequiresTwoFactor,
	NotActivated,
	MustChangePassword,
	InternalError,
}
