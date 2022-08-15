using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Migrations
{
	public partial class AddIdentityServer : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "ApiResources",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Enabled = table.Column<bool>(type: "bit", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
					DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
					AllowedAccessTokenSigningAlgorithms = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ShowInDiscoveryDocument = table.Column<bool>(type: "bit", nullable: false),
					RequireResourceIndicator = table.Column<bool>(type: "bit", nullable: false),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
					LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: true),
					NonEditable = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiResources", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "ApiScopes",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Enabled = table.Column<bool>(type: "bit", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
					DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Required = table.Column<bool>(type: "bit", nullable: false),
					Emphasize = table.Column<bool>(type: "bit", nullable: false),
					ShowInDiscoveryDocument = table.Column<bool>(type: "bit", nullable: false),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
					LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: true),
					NonEditable = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiScopes", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AspNetRoles",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoles", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUsers",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Bypass2faForExternalLogin = table.Column<bool>(type: "bit", nullable: false),
					UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
					PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
					TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
					LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
					AccessFailedCount = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUsers", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Clients",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Enabled = table.Column<bool>(type: "bit", nullable: false),
					ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ProtocolType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					RequireClientSecret = table.Column<bool>(type: "bit", nullable: false),
					ClientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClientUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
					LogoUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
					RequireConsent = table.Column<bool>(type: "bit", nullable: false),
					AllowRememberConsent = table.Column<bool>(type: "bit", nullable: false),
					AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(type: "bit", nullable: false),
					RequirePkce = table.Column<bool>(type: "bit", nullable: false),
					AllowPlainTextPkce = table.Column<bool>(type: "bit", nullable: false),
					RequireRequestObject = table.Column<bool>(type: "bit", nullable: false),
					AllowAccessTokensViaBrowser = table.Column<bool>(type: "bit", nullable: false),
					FrontChannelLogoutUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
					FrontChannelLogoutSessionRequired = table.Column<bool>(type: "bit", nullable: false),
					BackChannelLogoutUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
					BackChannelLogoutSessionRequired = table.Column<bool>(type: "bit", nullable: false),
					AllowOfflineAccess = table.Column<bool>(type: "bit", nullable: false),
					IdentityTokenLifetime = table.Column<int>(type: "int", nullable: false),
					AllowedIdentityTokenSigningAlgorithms = table.Column<string>(type: "nvarchar(max)", nullable: true),
					AccessTokenLifetime = table.Column<int>(type: "int", nullable: false),
					AuthorizationCodeLifetime = table.Column<int>(type: "int", nullable: false),
					ConsentLifetime = table.Column<int>(type: "int", nullable: true),
					AbsoluteRefreshTokenLifetime = table.Column<int>(type: "int", nullable: false),
					SlidingRefreshTokenLifetime = table.Column<int>(type: "int", nullable: false),
					RefreshTokenUsage = table.Column<int>(type: "int", nullable: false),
					UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(type: "bit", nullable: false),
					RefreshTokenExpiration = table.Column<int>(type: "int", nullable: false),
					AccessTokenType = table.Column<int>(type: "int", nullable: false),
					EnableLocalLogin = table.Column<bool>(type: "bit", nullable: false),
					IncludeJwtId = table.Column<bool>(type: "bit", nullable: false),
					AlwaysSendClientClaims = table.Column<bool>(type: "bit", nullable: false),
					ClientClaimsPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PairWiseSubjectSalt = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UserSsoLifetime = table.Column<int>(type: "int", nullable: true),
					UserCodeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					DeviceCodeLifetime = table.Column<int>(type: "int", nullable: false),
					CibaLifetime = table.Column<int>(type: "int", nullable: true),
					PollingInterval = table.Column<int>(type: "int", nullable: true),
					CoordinateLifetimeWithUserSession = table.Column<bool>(type: "bit", nullable: true),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
					LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: true),
					NonEditable = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Clients", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "DeviceFlowCodes",
				columns: table => new
				{
					DeviceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UserCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SubjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
					Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
				});

			migrationBuilder.CreateTable(
				name: "IdentityProviders",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Scheme = table.Column<string>(type: "nvarchar(max)", nullable: true),
					DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Enabled = table.Column<bool>(type: "bit", nullable: false),
					Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
					LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: true),
					NonEditable = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_IdentityProviders", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "IdentityResources",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Enabled = table.Column<bool>(type: "bit", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
					DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Required = table.Column<bool>(type: "bit", nullable: false),
					Emphasize = table.Column<bool>(type: "bit", nullable: false),
					ShowInDiscoveryDocument = table.Column<bool>(type: "bit", nullable: false),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
					NonEditable = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_IdentityResources", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Keys",
				columns: table => new
				{
					Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Version = table.Column<int>(type: "int", nullable: false),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					Use = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Algorithm = table.Column<string>(type: "nvarchar(max)", nullable: true),
					IsX509Certificate = table.Column<bool>(type: "bit", nullable: false),
					DataProtected = table.Column<bool>(type: "bit", nullable: false),
					Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Keys", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "PersistedGrants",
				columns: table => new
				{
					Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Id = table.Column<long>(type: "bigint", nullable: false),
					Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SubjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
					ConsumedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
					Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PersistedGrants", x => x.Key);
				});

			migrationBuilder.CreateTable(
				name: "ServerSideSessions",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Scheme = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SubjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
					DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					Renewed = table.Column<DateTime>(type: "datetime2", nullable: false),
					Expires = table.Column<DateTime>(type: "datetime2", nullable: true),
					Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ServerSideSessions", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "ApiResourceClaim",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ApiResourceId = table.Column<int>(type: "int", nullable: false),
					Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiResourceClaim", x => x.Id);
					table.ForeignKey(
						name: "FK_ApiResourceClaim_ApiResources_ApiResourceId",
						column: x => x.ApiResourceId,
						principalTable: "ApiResources",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ApiResourceProperty",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ApiResourceId = table.Column<int>(type: "int", nullable: false),
					Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiResourceProperty", x => x.Id);
					table.ForeignKey(
						name: "FK_ApiResourceProperty_ApiResources_ApiResourceId",
						column: x => x.ApiResourceId,
						principalTable: "ApiResources",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ApiResourceScope",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Scope = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ApiResourceId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiResourceScope", x => x.Id);
					table.ForeignKey(
						name: "FK_ApiResourceScope_ApiResources_ApiResourceId",
						column: x => x.ApiResourceId,
						principalTable: "ApiResources",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ApiResourceSecret",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ApiResourceId = table.Column<int>(type: "int", nullable: false),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
					Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiResourceSecret", x => x.Id);
					table.ForeignKey(
						name: "FK_ApiResourceSecret_ApiResources_ApiResourceId",
						column: x => x.ApiResourceId,
						principalTable: "ApiResources",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ApiScopeClaim",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ScopeId = table.Column<int>(type: "int", nullable: false),
					Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiScopeClaim", x => x.Id);
					table.ForeignKey(
						name: "FK_ApiScopeClaim_ApiScopes_ScopeId",
						column: x => x.ScopeId,
						principalTable: "ApiScopes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ApiScopeProperty",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ScopeId = table.Column<int>(type: "int", nullable: false),
					Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiScopeProperty", x => x.Id);
					table.ForeignKey(
						name: "FK_ApiScopeProperty_ApiScopes_ScopeId",
						column: x => x.ScopeId,
						principalTable: "ApiScopes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetRoleClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetUserClaims_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserLogins",
				columns: table => new
				{
					LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
					table.ForeignKey(
						name: "FK_AspNetUserLogins_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserRoles",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserTokens",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
					table.ForeignKey(
						name: "FK_AspNetUserTokens_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientClaim",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClientId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientClaim", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientClaim_Clients_ClientId",
						column: x => x.ClientId,
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientCorsOrigins",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClientId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientCorsOrigins", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientCorsOrigins_Clients_ClientId",
						column: x => x.ClientId,
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientGrantType",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					GrantType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClientId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientGrantType", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientGrantType_Clients_ClientId",
						column: x => x.ClientId,
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientIdPRestriction",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClientId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientIdPRestriction", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientIdPRestriction_Clients_ClientId",
						column: x => x.ClientId,
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientPostLogoutRedirectUri",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					PostLogoutRedirectUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClientId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientPostLogoutRedirectUri", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientPostLogoutRedirectUri_Clients_ClientId",
						column: x => x.ClientId,
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientProperty",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ClientId = table.Column<int>(type: "int", nullable: false),
					Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientProperty", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientProperty_Clients_ClientId",
						column: x => x.ClientId,
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientRedirectUri",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					RedirectUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClientId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientRedirectUri", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientRedirectUri_Clients_ClientId",
						column: x => x.ClientId,
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientScope",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Scope = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClientId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientScope", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientScope_Clients_ClientId",
						column: x => x.ClientId,
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientSecret",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ClientId = table.Column<int>(type: "int", nullable: false),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
					Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientSecret", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientSecret_Clients_ClientId",
						column: x => x.ClientId,
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "IdentityResourceClaim",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					IdentityResourceId = table.Column<int>(type: "int", nullable: false),
					Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_IdentityResourceClaim", x => x.Id);
					table.ForeignKey(
						name: "FK_IdentityResourceClaim_IdentityResources_IdentityResourceId",
						column: x => x.IdentityResourceId,
						principalTable: "IdentityResources",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "IdentityResourceProperty",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					IdentityResourceId = table.Column<int>(type: "int", nullable: false),
					Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_IdentityResourceProperty", x => x.Id);
					table.ForeignKey(
						name: "FK_IdentityResourceProperty_IdentityResources_IdentityResourceId",
						column: x => x.IdentityResourceId,
						principalTable: "IdentityResources",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_ApiResourceClaim_ApiResourceId",
				table: "ApiResourceClaim",
				column: "ApiResourceId");

			migrationBuilder.CreateIndex(
				name: "IX_ApiResourceProperty_ApiResourceId",
				table: "ApiResourceProperty",
				column: "ApiResourceId");

			migrationBuilder.CreateIndex(
				name: "IX_ApiResourceScope_ApiResourceId",
				table: "ApiResourceScope",
				column: "ApiResourceId");

			migrationBuilder.CreateIndex(
				name: "IX_ApiResourceSecret_ApiResourceId",
				table: "ApiResourceSecret",
				column: "ApiResourceId");

			migrationBuilder.CreateIndex(
				name: "IX_ApiScopeClaim_ScopeId",
				table: "ApiScopeClaim",
				column: "ScopeId");

			migrationBuilder.CreateIndex(
				name: "IX_ApiScopeProperty_ScopeId",
				table: "ApiScopeProperty",
				column: "ScopeId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetRoleClaims_RoleId",
				table: "AspNetRoleClaims",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles",
				column: "NormalizedName",
				unique: true,
				filter: "[NormalizedName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserClaims_UserId",
				table: "AspNetUserClaims",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserLogins_UserId",
				table: "AspNetUserLogins",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserRoles_RoleId",
				table: "AspNetUserRoles",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "EmailIndex",
				table: "AspNetUsers",
				column: "NormalizedEmail");

			migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				table: "AspNetUsers",
				column: "NormalizedUserName",
				unique: true,
				filter: "[NormalizedUserName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_ClientClaim_ClientId",
				table: "ClientClaim",
				column: "ClientId");

			migrationBuilder.CreateIndex(
				name: "IX_ClientCorsOrigins_ClientId",
				table: "ClientCorsOrigins",
				column: "ClientId");

			migrationBuilder.CreateIndex(
				name: "IX_ClientGrantType_ClientId",
				table: "ClientGrantType",
				column: "ClientId");

			migrationBuilder.CreateIndex(
				name: "IX_ClientIdPRestriction_ClientId",
				table: "ClientIdPRestriction",
				column: "ClientId");

			migrationBuilder.CreateIndex(
				name: "IX_ClientPostLogoutRedirectUri_ClientId",
				table: "ClientPostLogoutRedirectUri",
				column: "ClientId");

			migrationBuilder.CreateIndex(
				name: "IX_ClientProperty_ClientId",
				table: "ClientProperty",
				column: "ClientId");

			migrationBuilder.CreateIndex(
				name: "IX_ClientRedirectUri_ClientId",
				table: "ClientRedirectUri",
				column: "ClientId");

			migrationBuilder.CreateIndex(
				name: "IX_ClientScope_ClientId",
				table: "ClientScope",
				column: "ClientId");

			migrationBuilder.CreateIndex(
				name: "IX_ClientSecret_ClientId",
				table: "ClientSecret",
				column: "ClientId");

			migrationBuilder.CreateIndex(
				name: "IX_IdentityResourceClaim_IdentityResourceId",
				table: "IdentityResourceClaim",
				column: "IdentityResourceId");

			migrationBuilder.CreateIndex(
				name: "IX_IdentityResourceProperty_IdentityResourceId",
				table: "IdentityResourceProperty",
				column: "IdentityResourceId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ApiResourceClaim");

			migrationBuilder.DropTable(
				name: "ApiResourceProperty");

			migrationBuilder.DropTable(
				name: "ApiResourceScope");

			migrationBuilder.DropTable(
				name: "ApiResourceSecret");

			migrationBuilder.DropTable(
				name: "ApiScopeClaim");

			migrationBuilder.DropTable(
				name: "ApiScopeProperty");

			migrationBuilder.DropTable(
				name: "AspNetRoleClaims");

			migrationBuilder.DropTable(
				name: "AspNetUserClaims");

			migrationBuilder.DropTable(
				name: "AspNetUserLogins");

			migrationBuilder.DropTable(
				name: "AspNetUserRoles");

			migrationBuilder.DropTable(
				name: "AspNetUserTokens");

			migrationBuilder.DropTable(
				name: "ClientClaim");

			migrationBuilder.DropTable(
				name: "ClientCorsOrigins");

			migrationBuilder.DropTable(
				name: "ClientGrantType");

			migrationBuilder.DropTable(
				name: "ClientIdPRestriction");

			migrationBuilder.DropTable(
				name: "ClientPostLogoutRedirectUri");

			migrationBuilder.DropTable(
				name: "ClientProperty");

			migrationBuilder.DropTable(
				name: "ClientRedirectUri");

			migrationBuilder.DropTable(
				name: "ClientScope");

			migrationBuilder.DropTable(
				name: "ClientSecret");

			migrationBuilder.DropTable(
				name: "DeviceFlowCodes");

			migrationBuilder.DropTable(
				name: "IdentityProviders");

			migrationBuilder.DropTable(
				name: "IdentityResourceClaim");

			migrationBuilder.DropTable(
				name: "IdentityResourceProperty");

			migrationBuilder.DropTable(
				name: "Keys");

			migrationBuilder.DropTable(
				name: "PersistedGrants");

			migrationBuilder.DropTable(
				name: "ServerSideSessions");

			migrationBuilder.DropTable(
				name: "ApiResources");

			migrationBuilder.DropTable(
				name: "ApiScopes");

			migrationBuilder.DropTable(
				name: "AspNetRoles");

			migrationBuilder.DropTable(
				name: "AspNetUsers");

			migrationBuilder.DropTable(
				name: "Clients");

			migrationBuilder.DropTable(
				name: "IdentityResources");
		}
	}
}
