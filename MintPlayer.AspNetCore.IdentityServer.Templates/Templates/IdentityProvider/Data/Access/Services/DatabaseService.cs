using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Options;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;
using System.Net.Mail;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Services;

internal class DatabaseService : IDatabaseService
{
	#region Constructor
	private readonly SsoContext ssoContext;
	public DatabaseService(SsoContext ssoContext)
	{
		this.ssoContext = ssoContext;
	}
	#endregion

	public void Migrate()
	{
		ssoContext.Database.Migrate();
	}
}
