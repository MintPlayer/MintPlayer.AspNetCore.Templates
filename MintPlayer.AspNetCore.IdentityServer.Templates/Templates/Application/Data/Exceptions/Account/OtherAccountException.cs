using Microsoft.AspNetCore.Identity;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Exceptions.Account;

public class OtherAccountException : Exception
{
	public OtherAccountException(IList<UserLoginInfo> existingLogins)
		: base($"Please login with your {existingLogins[0].ProviderDisplayName} account instead")
	{
	}
}
