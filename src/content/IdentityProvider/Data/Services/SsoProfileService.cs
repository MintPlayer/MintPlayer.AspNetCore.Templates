using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance.Entities;
using System.Security.Claims;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Services;

internal class SsoProfileService : IProfileService
{
	#region Constructor
	private readonly UserManager<User> userManager;
	public SsoProfileService(UserManager<Persistance.Entities.User> userManager)
	{
		this.userManager = userManager;
	}
	#endregion

	public async Task GetProfileDataAsync(ProfileDataRequestContext context)
	{
		var user = await userManager.GetUserAsync(context.Subject);
		var allClaimsFromDatabase = await userManager.GetClaimsAsync(user);
		context.IssuedClaims = context.RequestedClaimTypes
			.Select(ct =>
			{
				switch (ct)
				{
					case ClaimTypes.Name:
						if (string.IsNullOrEmpty(user.UserName)) return null;
						else return new System.Security.Claims.Claim(ClaimTypes.Name, user.UserName);
					case ClaimTypes.Email:
						if (string.IsNullOrEmpty(user.Email)) return null;
						else return new System.Security.Claims.Claim(ClaimTypes.Email, user.Email);
					case ClaimTypes.MobilePhone:
						if (string.IsNullOrEmpty(user.PhoneNumber)) return null;
						else return new System.Security.Claims.Claim(ClaimTypes.MobilePhone, user.PhoneNumber);
					default:
						return allClaimsFromDatabase.FirstOrDefault(c => c.Type == ct);
				}
			})
			.Where(c => c != null)
			.ToList();
	}

	public async Task IsActiveAsync(IsActiveContext context)
	{
		var user = await userManager.GetUserAsync(context.Subject);
		var emailConfirmed = await userManager.IsEmailConfirmedAsync(user);
		var phoneConfirmed = await userManager.IsPhoneNumberConfirmedAsync(user);
		context.IsActive = emailConfirmed || phoneConfirmed;
	}
}
