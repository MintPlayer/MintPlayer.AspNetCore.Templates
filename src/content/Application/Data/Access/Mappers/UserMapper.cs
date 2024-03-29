namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Access.Mappers;

internal class UserMapper : IUserMapper
{
	public Task<Persistance.Entities.User?> Dto2Entity(Dtos.Dtos.User? user)
	{
		if (user == null)
		{
			return Task.FromResult<Persistance.Entities.User?>(null);
		}
		else
		{
			return Task.FromResult<Persistance.Entities.User?>(new Persistance.Entities.User
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName,
				TwoFactorEnabled = user.IsTwoFactorEnabled,
				Bypass2faForExternalLogin = user.Bypass2faForExternalLogin,
			});
		}
	}

	public Task<Dtos.Dtos.User?> Entity2Dto(Persistance.Entities.User? user)
	{
		if (user == null)
		{
			return Task.FromResult<Dtos.Dtos.User?>(null);
		}
		else
		{
			return Task.FromResult<Dtos.Dtos.User?>(new Dtos.Dtos.User
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
