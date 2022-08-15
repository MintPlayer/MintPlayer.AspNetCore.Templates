using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Microsoft.Extensions.DependencyInjection;
using MintPlayer.Random.Abstractions;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Repositories;

internal class SetupRepository : ISetupRepository
{
	#region Constructor
	private readonly SsoContext ssoContext;
	private readonly IServiceProvider serviceProvider;
	public SetupRepository(Data.Persistance.SsoContext ssoContext, IServiceProvider serviceProvider)
	{
		this.ssoContext = ssoContext;
		this.serviceProvider = serviceProvider;
	}
	#endregion

	public Task<bool> IsDeveloperPortalRegistered()
	{
		var appsCount = ssoContext.Clients.Count();
		return Task.FromResult(appsCount > 0);
	}

	public async Task<CreateDeveloperPortalResponse> CreateDeveloperClient(CreateDeveloperPortalRequest request)
	{
		if (ssoContext.Clients.Any())
		{
			throw new Exception("There are already clients in the database");
		}

		var defaultSecret = string.Empty;
		using (var scope = serviceProvider.CreateScope())
		{
			var randomizer = serviceProvider.GetRequiredService<IRandomizer>();
			defaultSecret = await randomizer.RandomString(30);
		}
		if (default == string.Empty)
		{
			throw new Exception();
		}

		var client = new Duende.IdentityServer.EntityFramework.Entities.Client
		{
			Enabled = true,
			ClientId = Guid.NewGuid().ToString(),
			ProtocolType = Duende.IdentityServer.IdentityServerConstants.ProtocolTypes.OpenIdConnect,
			RequireClientSecret = true,
			ClientName = "DeveloperPortal",
			Description = "Client for the developer portal",
			//ClientUri = request.DeveloperPortalUrl,
			//LogoUri = null,
			RequireConsent = false,
			AllowRememberConsent = true,
			AlwaysIncludeUserClaimsInIdToken = true,
			RequirePkce = true,
			AllowPlainTextPkce = true, // false
			RequireRequestObject = false,
			AllowAccessTokensViaBrowser = true,
			FrontChannelLogoutUri = null,
			FrontChannelLogoutSessionRequired = false,
			BackChannelLogoutUri = null,
			BackChannelLogoutSessionRequired = false,
			AllowOfflineAccess = true,
			IdentityTokenLifetime = 86400,
			AllowedIdentityTokenSigningAlgorithms = null,
			AccessTokenLifetime = 86400,
			AuthorizationCodeLifetime = 86400,
			ConsentLifetime = 86400,
			AbsoluteRefreshTokenLifetime = 86400,
			SlidingRefreshTokenLifetime = 86400,
			RefreshTokenUsage = (int)TokenUsage.OneTimeOnly,
			UpdateAccessTokenClaimsOnRefresh = true,
			RefreshTokenExpiration = 86400,
			AccessTokenType = (int)AccessTokenType.Jwt,
			EnableLocalLogin = true,
			IncludeJwtId = true,
			AlwaysSendClientClaims = true,
			ClientClaimsPrefix = "dev-",
			PairWiseSubjectSalt = null,
			Created = DateTime.UtcNow,
			Updated = null,
			LastAccessed = null,
			UserSsoLifetime = 86400,
			UserCodeType = "bearer",
			DeviceCodeLifetime = 86400,
			NonEditable = true,
			AllowedScopes = new List<Duende.IdentityServer.EntityFramework.Entities.ClientScope>
			{
				new Duende.IdentityServer.EntityFramework.Entities.ClientScope { Scope = "openid" },
				new Duende.IdentityServer.EntityFramework.Entities.ClientScope { Scope = "profile" },
				new Duende.IdentityServer.EntityFramework.Entities.ClientScope { Scope = "email" },
			},
			ClientSecrets = new List<Duende.IdentityServer.EntityFramework.Entities.ClientSecret>
			{
				new Duende.IdentityServer.EntityFramework.Entities.ClientSecret
				{
					Created = DateTime.Now,
					Description = "Default secret to create the developer portal",
					Type = Duende.IdentityServer.IdentityServerConstants.SecretTypes.SharedSecret,
					Value = defaultSecret.Sha256(),
				}
			},
			RedirectUris = request.RedirectUris.Select(r => new Duende.IdentityServer.EntityFramework.Entities.ClientRedirectUri
			{
				RedirectUri = r,
			}).ToList(),
			AllowedGrantTypes = GrantTypes.CodeAndClientCredentials.Select(gt => new Duende.IdentityServer.EntityFramework.Entities.ClientGrantType { GrantType = gt }).ToList(),
		};

		await ssoContext.Clients.AddAsync(client);

		if (!ssoContext.IdentityResources.Any())
		{
			await ssoContext.IdentityResources.AddRangeAsync(
				new Duende.IdentityServer.EntityFramework.Entities.IdentityResource
				{
					Enabled = true,
					Name = "openid",
					DisplayName = "Your user identifier",
					Description = "Your user identifier",
					Required = true,
					ShowInDiscoveryDocument = true,
					UserClaims = new List<Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim>
					{
						new Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim { Type = "id" },
						new Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim { Type = "name" },
					}
				},
				new Duende.IdentityServer.EntityFramework.Entities.IdentityResource
				{
					Enabled = true,
					Name = "profile",
					DisplayName = "User profile",
					Description = "Your user profile information (first name, last name, etc.)",
					Required = false,
					ShowInDiscoveryDocument = true,
					UserClaims = new List<Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim>
					{
					}
				},
				new Duende.IdentityServer.EntityFramework.Entities.IdentityResource
				{
					Enabled = true,
					Name = "email",
					DisplayName = "Your email address",
					Description = "Your email address",
					Required = false,
					ShowInDiscoveryDocument = true,
					UserClaims = new List<Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim>
					{
						new Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim { Type = "email" },
					}
				},
				new Duende.IdentityServer.EntityFramework.Entities.IdentityResource
				{
					Enabled = true,
					Name = "phone",
					DisplayName = "Your phone number",
					Description = "Your phone number",
					Required = false,
					ShowInDiscoveryDocument = true,
					UserClaims = new List<Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim>
					{
						//new Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim { Type = "mobilephone" },
					}
				}
			);
		}

		await ssoContext.SaveChangesAsync();

		return new CreateDeveloperPortalResponse
		{
			ClientId = client.ClientId,
			ClientSecret = defaultSecret,
		};
	}
}
