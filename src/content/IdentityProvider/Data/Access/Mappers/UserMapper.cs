namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Mappers;

internal class UserMapper : IUserMapper
{
	public Task<Persistance.Entities.User> Dto2Entity(Dtos.Dtos.User user)
	{
		if (user == null)
		{
			return null;
		}
		else
		{
			return Task.FromResult(new Persistance.Entities.User
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName,
				TwoFactorEnabled = user.IsTwoFactorEnabled,
				Bypass2faForExternalLogin = user.Bypass2faForExternalLogin,
			});
		}
	}

	public Task<Dtos.Dtos.User> Entity2Dto(Persistance.Entities.User user)
	{
		if (user == null)
		{
			return null;
		}
		else
		{
			return Task.FromResult(new Dtos.Dtos.User
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName,
				IsTwoFactorEnabled = user.TwoFactorEnabled,
				Bypass2faForExternalLogin = user.Bypass2faForExternalLogin,
			});
		}
	}
}
