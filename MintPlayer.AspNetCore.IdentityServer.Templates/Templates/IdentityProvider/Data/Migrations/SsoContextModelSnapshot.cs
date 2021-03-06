// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance;

#nullable disable

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Migrations
{
	[DbContext(typeof(SsoContext))]
	partial class SsoContextModelSnapshot : ModelSnapshot
	{
		protected override void BuildModel(ModelBuilder modelBuilder)
		{
#pragma warning disable 612, 618
			modelBuilder
				.HasAnnotation("ProductVersion", "6.0.6")
				.HasAnnotation("Relational:MaxIdentifierLength", 128);

			SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResource", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<string>("AllowedAccessTokenSigningAlgorithms")
						.HasColumnType("nvarchar(max)");

					b.Property<DateTime>("Created")
						.HasColumnType("datetime2");

					b.Property<string>("Description")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("DisplayName")
						.HasColumnType("nvarchar(max)");

					b.Property<bool>("Enabled")
						.HasColumnType("bit");

					b.Property<DateTime?>("LastAccessed")
						.HasColumnType("datetime2");

					b.Property<string>("Name")
						.HasColumnType("nvarchar(max)");

					b.Property<bool>("NonEditable")
						.HasColumnType("bit");

					b.Property<bool>("RequireResourceIndicator")
						.HasColumnType("bit");

					b.Property<bool>("ShowInDiscoveryDocument")
						.HasColumnType("bit");

					b.Property<DateTime?>("Updated")
						.HasColumnType("datetime2");

					b.HasKey("Id");

					b.ToTable("ApiResources");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceClaim", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ApiResourceId")
						.HasColumnType("int");

					b.Property<string>("Type")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ApiResourceId");

					b.ToTable("ApiResourceClaim");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceProperty", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ApiResourceId")
						.HasColumnType("int");

					b.Property<string>("Key")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Value")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ApiResourceId");

					b.ToTable("ApiResourceProperty");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceScope", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ApiResourceId")
						.HasColumnType("int");

					b.Property<string>("Scope")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ApiResourceId");

					b.ToTable("ApiResourceScope");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceSecret", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ApiResourceId")
						.HasColumnType("int");

					b.Property<DateTime>("Created")
						.HasColumnType("datetime2");

					b.Property<string>("Description")
						.HasColumnType("nvarchar(max)");

					b.Property<DateTime?>("Expiration")
						.HasColumnType("datetime2");

					b.Property<string>("Type")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Value")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ApiResourceId");

					b.ToTable("ApiResourceSecret");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScope", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<DateTime>("Created")
						.HasColumnType("datetime2");

					b.Property<string>("Description")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("DisplayName")
						.HasColumnType("nvarchar(max)");

					b.Property<bool>("Emphasize")
						.HasColumnType("bit");

					b.Property<bool>("Enabled")
						.HasColumnType("bit");

					b.Property<DateTime?>("LastAccessed")
						.HasColumnType("datetime2");

					b.Property<string>("Name")
						.HasColumnType("nvarchar(max)");

					b.Property<bool>("NonEditable")
						.HasColumnType("bit");

					b.Property<bool>("Required")
						.HasColumnType("bit");

					b.Property<bool>("ShowInDiscoveryDocument")
						.HasColumnType("bit");

					b.Property<DateTime?>("Updated")
						.HasColumnType("datetime2");

					b.HasKey("Id");

					b.ToTable("ApiScopes");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeClaim", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ScopeId")
						.HasColumnType("int");

					b.Property<string>("Type")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ScopeId");

					b.ToTable("ApiScopeClaim");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeProperty", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<string>("Key")
						.HasColumnType("nvarchar(max)");

					b.Property<int>("ScopeId")
						.HasColumnType("int");

					b.Property<string>("Value")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ScopeId");

					b.ToTable("ApiScopeProperty");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.Client", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("AbsoluteRefreshTokenLifetime")
						.HasColumnType("int");

					b.Property<int>("AccessTokenLifetime")
						.HasColumnType("int");

					b.Property<int>("AccessTokenType")
						.HasColumnType("int");

					b.Property<bool>("AllowAccessTokensViaBrowser")
						.HasColumnType("bit");

					b.Property<bool>("AllowOfflineAccess")
						.HasColumnType("bit");

					b.Property<bool>("AllowPlainTextPkce")
						.HasColumnType("bit");

					b.Property<bool>("AllowRememberConsent")
						.HasColumnType("bit");

					b.Property<string>("AllowedIdentityTokenSigningAlgorithms")
						.HasColumnType("nvarchar(max)");

					b.Property<bool>("AlwaysIncludeUserClaimsInIdToken")
						.HasColumnType("bit");

					b.Property<bool>("AlwaysSendClientClaims")
						.HasColumnType("bit");

					b.Property<int>("AuthorizationCodeLifetime")
						.HasColumnType("int");

					b.Property<bool>("BackChannelLogoutSessionRequired")
						.HasColumnType("bit");

					b.Property<string>("BackChannelLogoutUri")
						.HasColumnType("nvarchar(max)");

					b.Property<int?>("CibaLifetime")
						.HasColumnType("int");

					b.Property<string>("ClientClaimsPrefix")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("ClientId")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("ClientName")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("ClientUri")
						.HasColumnType("nvarchar(max)");

					b.Property<int?>("ConsentLifetime")
						.HasColumnType("int");

					b.Property<bool?>("CoordinateLifetimeWithUserSession")
						.HasColumnType("bit");

					b.Property<DateTime>("Created")
						.HasColumnType("datetime2");

					b.Property<string>("Description")
						.HasColumnType("nvarchar(max)");

					b.Property<int>("DeviceCodeLifetime")
						.HasColumnType("int");

					b.Property<bool>("EnableLocalLogin")
						.HasColumnType("bit");

					b.Property<bool>("Enabled")
						.HasColumnType("bit");

					b.Property<bool>("FrontChannelLogoutSessionRequired")
						.HasColumnType("bit");

					b.Property<string>("FrontChannelLogoutUri")
						.HasColumnType("nvarchar(max)");

					b.Property<int>("IdentityTokenLifetime")
						.HasColumnType("int");

					b.Property<bool>("IncludeJwtId")
						.HasColumnType("bit");

					b.Property<DateTime?>("LastAccessed")
						.HasColumnType("datetime2");

					b.Property<string>("LogoUri")
						.HasColumnType("nvarchar(max)");

					b.Property<bool>("NonEditable")
						.HasColumnType("bit");

					b.Property<string>("PairWiseSubjectSalt")
						.HasColumnType("nvarchar(max)");

					b.Property<int?>("PollingInterval")
						.HasColumnType("int");

					b.Property<string>("ProtocolType")
						.HasColumnType("nvarchar(max)");

					b.Property<int>("RefreshTokenExpiration")
						.HasColumnType("int");

					b.Property<int>("RefreshTokenUsage")
						.HasColumnType("int");

					b.Property<bool>("RequireClientSecret")
						.HasColumnType("bit");

					b.Property<bool>("RequireConsent")
						.HasColumnType("bit");

					b.Property<bool>("RequirePkce")
						.HasColumnType("bit");

					b.Property<bool>("RequireRequestObject")
						.HasColumnType("bit");

					b.Property<int>("SlidingRefreshTokenLifetime")
						.HasColumnType("int");

					b.Property<bool>("UpdateAccessTokenClaimsOnRefresh")
						.HasColumnType("bit");

					b.Property<DateTime?>("Updated")
						.HasColumnType("datetime2");

					b.Property<string>("UserCodeType")
						.HasColumnType("nvarchar(max)");

					b.Property<int?>("UserSsoLifetime")
						.HasColumnType("int");

					b.HasKey("Id");

					b.ToTable("Clients");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientClaim", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ClientId")
						.HasColumnType("int");

					b.Property<string>("Type")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Value")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ClientId");

					b.ToTable("ClientClaim");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientCorsOrigin", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ClientId")
						.HasColumnType("int");

					b.Property<string>("Origin")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ClientId");

					b.ToTable("ClientCorsOrigins");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientGrantType", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ClientId")
						.HasColumnType("int");

					b.Property<string>("GrantType")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ClientId");

					b.ToTable("ClientGrantType");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientIdPRestriction", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ClientId")
						.HasColumnType("int");

					b.Property<string>("Provider")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ClientId");

					b.ToTable("ClientIdPRestriction");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientPostLogoutRedirectUri", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ClientId")
						.HasColumnType("int");

					b.Property<string>("PostLogoutRedirectUri")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ClientId");

					b.ToTable("ClientPostLogoutRedirectUri");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientProperty", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ClientId")
						.HasColumnType("int");

					b.Property<string>("Key")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Value")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ClientId");

					b.ToTable("ClientProperty");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientRedirectUri", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ClientId")
						.HasColumnType("int");

					b.Property<string>("RedirectUri")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ClientId");

					b.ToTable("ClientRedirectUri");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientScope", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ClientId")
						.HasColumnType("int");

					b.Property<string>("Scope")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ClientId");

					b.ToTable("ClientScope");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientSecret", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("ClientId")
						.HasColumnType("int");

					b.Property<DateTime>("Created")
						.HasColumnType("datetime2");

					b.Property<string>("Description")
						.HasColumnType("nvarchar(max)");

					b.Property<DateTime?>("Expiration")
						.HasColumnType("datetime2");

					b.Property<string>("Type")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Value")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("ClientId");

					b.ToTable("ClientSecret");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.DeviceFlowCodes", b =>
				{
					b.Property<string>("ClientId")
						.HasColumnType("nvarchar(max)");

					b.Property<DateTime>("CreationTime")
						.HasColumnType("datetime2");

					b.Property<string>("Data")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Description")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("DeviceCode")
						.HasColumnType("nvarchar(max)");

					b.Property<DateTime?>("Expiration")
						.HasColumnType("datetime2");

					b.Property<string>("SessionId")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("SubjectId")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("UserCode")
						.HasColumnType("nvarchar(max)");

					b.ToTable("DeviceFlowCodes");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityProvider", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<DateTime>("Created")
						.HasColumnType("datetime2");

					b.Property<string>("DisplayName")
						.HasColumnType("nvarchar(max)");

					b.Property<bool>("Enabled")
						.HasColumnType("bit");

					b.Property<DateTime?>("LastAccessed")
						.HasColumnType("datetime2");

					b.Property<bool>("NonEditable")
						.HasColumnType("bit");

					b.Property<string>("Properties")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Scheme")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Type")
						.HasColumnType("nvarchar(max)");

					b.Property<DateTime?>("Updated")
						.HasColumnType("datetime2");

					b.HasKey("Id");

					b.ToTable("IdentityProviders");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<DateTime>("Created")
						.HasColumnType("datetime2");

					b.Property<string>("Description")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("DisplayName")
						.HasColumnType("nvarchar(max)");

					b.Property<bool>("Emphasize")
						.HasColumnType("bit");

					b.Property<bool>("Enabled")
						.HasColumnType("bit");

					b.Property<string>("Name")
						.HasColumnType("nvarchar(max)");

					b.Property<bool>("NonEditable")
						.HasColumnType("bit");

					b.Property<bool>("Required")
						.HasColumnType("bit");

					b.Property<bool>("ShowInDiscoveryDocument")
						.HasColumnType("bit");

					b.Property<DateTime?>("Updated")
						.HasColumnType("datetime2");

					b.HasKey("Id");

					b.ToTable("IdentityResources");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("IdentityResourceId")
						.HasColumnType("int");

					b.Property<string>("Type")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("IdentityResourceId");

					b.ToTable("IdentityResourceClaim");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceProperty", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<int>("IdentityResourceId")
						.HasColumnType("int");

					b.Property<string>("Key")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Value")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.HasIndex("IdentityResourceId");

					b.ToTable("IdentityResourceProperty");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.Key", b =>
				{
					b.Property<string>("Id")
						.HasColumnType("nvarchar(450)");

					b.Property<string>("Algorithm")
						.HasColumnType("nvarchar(max)");

					b.Property<DateTime>("Created")
						.HasColumnType("datetime2");

					b.Property<string>("Data")
						.HasColumnType("nvarchar(max)");

					b.Property<bool>("DataProtected")
						.HasColumnType("bit");

					b.Property<bool>("IsX509Certificate")
						.HasColumnType("bit");

					b.Property<string>("Use")
						.HasColumnType("nvarchar(max)");

					b.Property<int>("Version")
						.HasColumnType("int");

					b.HasKey("Id");

					b.ToTable("Keys");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.PersistedGrant", b =>
				{
					b.Property<string>("Key")
						.HasColumnType("nvarchar(450)");

					b.Property<string>("ClientId")
						.HasColumnType("nvarchar(max)");

					b.Property<DateTime?>("ConsumedTime")
						.HasColumnType("datetime2");

					b.Property<DateTime>("CreationTime")
						.HasColumnType("datetime2");

					b.Property<string>("Data")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Description")
						.HasColumnType("nvarchar(max)");

					b.Property<DateTime?>("Expiration")
						.HasColumnType("datetime2");

					b.Property<long>("Id")
						.HasColumnType("bigint");

					b.Property<string>("SessionId")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("SubjectId")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Type")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Key");

					b.ToTable("PersistedGrants");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ServerSideSession", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<DateTime>("Created")
						.HasColumnType("datetime2");

					b.Property<string>("Data")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("DisplayName")
						.HasColumnType("nvarchar(max)");

					b.Property<DateTime?>("Expires")
						.HasColumnType("datetime2");

					b.Property<string>("Key")
						.HasColumnType("nvarchar(max)");

					b.Property<DateTime>("Renewed")
						.HasColumnType("datetime2");

					b.Property<string>("Scheme")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("SessionId")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("SubjectId")
						.HasColumnType("nvarchar(max)");

					b.HasKey("Id");

					b.ToTable("ServerSideSessions");
				});

			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<string>("ClaimType")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("ClaimValue")
						.HasColumnType("nvarchar(max)");

					b.Property<Guid>("RoleId")
						.HasColumnType("uniqueidentifier");

					b.HasKey("Id");

					b.HasIndex("RoleId");

					b.ToTable("AspNetRoleClaims", (string)null);
				});

			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("int");

					SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

					b.Property<string>("ClaimType")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("ClaimValue")
						.HasColumnType("nvarchar(max)");

					b.Property<Guid>("UserId")
						.HasColumnType("uniqueidentifier");

					b.HasKey("Id");

					b.HasIndex("UserId");

					b.ToTable("AspNetUserClaims", (string)null);
				});

			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
				{
					b.Property<string>("LoginProvider")
						.HasColumnType("nvarchar(450)");

					b.Property<string>("ProviderKey")
						.HasColumnType("nvarchar(450)");

					b.Property<string>("ProviderDisplayName")
						.HasColumnType("nvarchar(max)");

					b.Property<Guid>("UserId")
						.HasColumnType("uniqueidentifier");

					b.HasKey("LoginProvider", "ProviderKey");

					b.HasIndex("UserId");

					b.ToTable("AspNetUserLogins", (string)null);
				});

			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
				{
					b.Property<Guid>("UserId")
						.HasColumnType("uniqueidentifier");

					b.Property<Guid>("RoleId")
						.HasColumnType("uniqueidentifier");

					b.HasKey("UserId", "RoleId");

					b.HasIndex("RoleId");

					b.ToTable("AspNetUserRoles", (string)null);

					b.HasData(
						new
						{
							UserId = new Guid("d2c53e36-9a0a-42e4-b075-a4cc6481dd15"),
							RoleId = new Guid("0097e37b-1c43-47d6-9665-419aa28cd8be")
						});
				});

			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
				{
					b.Property<Guid>("UserId")
						.HasColumnType("uniqueidentifier");

					b.Property<string>("LoginProvider")
						.HasColumnType("nvarchar(450)");

					b.Property<string>("Name")
						.HasColumnType("nvarchar(450)");

					b.Property<string>("Value")
						.HasColumnType("nvarchar(max)");

					b.HasKey("UserId", "LoginProvider", "Name");

					b.ToTable("AspNetUserTokens", (string)null);
				});

			modelBuilder.Entity("MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance.Entities.Role", b =>
				{
					b.Property<Guid>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("uniqueidentifier");

					b.Property<string>("ConcurrencyStamp")
						.IsConcurrencyToken()
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Name")
						.HasMaxLength(256)
						.HasColumnType("nvarchar(256)");

					b.Property<string>("NormalizedName")
						.HasMaxLength(256)
						.HasColumnType("nvarchar(256)");

					b.HasKey("Id");

					b.HasIndex("NormalizedName")
						.IsUnique()
						.HasDatabaseName("RoleNameIndex")
						.HasFilter("[NormalizedName] IS NOT NULL");

					b.ToTable("AspNetRoles", (string)null);

					b.HasData(
						new
						{
							Id = new Guid("0097e37b-1c43-47d6-9665-419aa28cd8be"),
							ConcurrencyStamp = "1bb3c759-721f-44ea-9ff3-623a8cec7eab",
							Name = "Administrator",
							NormalizedName = "Administrator"
						});
				});

			modelBuilder.Entity("MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance.Entities.User", b =>
				{
					b.Property<Guid>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("uniqueidentifier");

					b.Property<int>("AccessFailedCount")
						.HasColumnType("int");

					b.Property<bool>("Bypass2faForExternalLogin")
						.HasColumnType("bit");

					b.Property<string>("ConcurrencyStamp")
						.IsConcurrencyToken()
						.HasColumnType("nvarchar(max)");

					b.Property<string>("Email")
						.HasMaxLength(256)
						.HasColumnType("nvarchar(256)");

					b.Property<bool>("EmailConfirmed")
						.HasColumnType("bit");

					b.Property<bool>("LockoutEnabled")
						.HasColumnType("bit");

					b.Property<DateTimeOffset?>("LockoutEnd")
						.HasColumnType("datetimeoffset");

					b.Property<string>("NormalizedEmail")
						.HasMaxLength(256)
						.HasColumnType("nvarchar(256)");

					b.Property<string>("NormalizedUserName")
						.HasMaxLength(256)
						.HasColumnType("nvarchar(256)");

					b.Property<string>("PasswordHash")
						.HasColumnType("nvarchar(max)");

					b.Property<string>("PhoneNumber")
						.HasColumnType("nvarchar(max)");

					b.Property<bool>("PhoneNumberConfirmed")
						.HasColumnType("bit");

					b.Property<string>("SecurityStamp")
						.HasColumnType("nvarchar(max)");

					b.Property<bool>("TwoFactorEnabled")
						.HasColumnType("bit");

					b.Property<string>("UserName")
						.HasMaxLength(256)
						.HasColumnType("nvarchar(256)");

					b.HasKey("Id");

					b.HasIndex("NormalizedEmail")
						.HasDatabaseName("EmailIndex");

					b.HasIndex("NormalizedUserName")
						.IsUnique()
						.HasDatabaseName("UserNameIndex")
						.HasFilter("[NormalizedUserName] IS NOT NULL");

					b.ToTable("AspNetUsers", (string)null);

					b.HasData(
						new
						{
							Id = new Guid("d2c53e36-9a0a-42e4-b075-a4cc6481dd15"),
							AccessFailedCount = 0,
							Bypass2faForExternalLogin = false,
							ConcurrencyStamp = "f125d4ba-ce9c-473b-a110-3c81e053c17b",
							Email = "admin@example.com",
							EmailConfirmed = true,
							LockoutEnabled = false,
							NormalizedEmail = "ADMIN@EXAMPLE.COM",
							NormalizedUserName = "admin",
							PasswordHash = "AQAAAAEAACcQAAAAEAxq56sy77JI+Q3N9GPxkCHd0pzLlyfnXY7vh9vueMFPDi/qyg7CUM7YBPLmFTSNUw==",
							PhoneNumberConfirmed = false,
							SecurityStamp = "",
							TwoFactorEnabled = false,
							UserName = "admin"
						});
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceClaim", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource")
						.WithMany("UserClaims")
						.HasForeignKey("ApiResourceId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("ApiResource");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceProperty", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource")
						.WithMany("Properties")
						.HasForeignKey("ApiResourceId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("ApiResource");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceScope", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource")
						.WithMany("Scopes")
						.HasForeignKey("ApiResourceId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("ApiResource");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceSecret", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource")
						.WithMany("Secrets")
						.HasForeignKey("ApiResourceId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("ApiResource");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeClaim", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiScope", "Scope")
						.WithMany("UserClaims")
						.HasForeignKey("ScopeId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Scope");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeProperty", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiScope", "Scope")
						.WithMany("Properties")
						.HasForeignKey("ScopeId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Scope");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientClaim", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("Claims")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientCorsOrigin", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("AllowedCorsOrigins")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientGrantType", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("AllowedGrantTypes")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientIdPRestriction", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("IdentityProviderRestrictions")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientPostLogoutRedirectUri", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("PostLogoutRedirectUris")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientProperty", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("Properties")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientRedirectUri", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("RedirectUris")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientScope", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("AllowedScopes")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientSecret", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("ClientSecrets")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", "IdentityResource")
						.WithMany("UserClaims")
						.HasForeignKey("IdentityResourceId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("IdentityResource");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceProperty", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", "IdentityResource")
						.WithMany("Properties")
						.HasForeignKey("IdentityResourceId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("IdentityResource");
				});

			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
				{
					b.HasOne("MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance.Entities.Role", null)
						.WithMany()
						.HasForeignKey("RoleId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();
				});

			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
				{
					b.HasOne("MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance.Entities.User", null)
						.WithMany()
						.HasForeignKey("UserId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();
				});

			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
				{
					b.HasOne("MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance.Entities.User", null)
						.WithMany()
						.HasForeignKey("UserId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();
				});

			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
				{
					b.HasOne("MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance.Entities.Role", null)
						.WithMany()
						.HasForeignKey("RoleId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.HasOne("MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance.Entities.User", null)
						.WithMany()
						.HasForeignKey("UserId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();
				});

			modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
				{
					b.HasOne("MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance.Entities.User", null)
						.WithMany()
						.HasForeignKey("UserId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResource", b =>
				{
					b.Navigation("Properties");

					b.Navigation("Scopes");

					b.Navigation("Secrets");

					b.Navigation("UserClaims");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScope", b =>
				{
					b.Navigation("Properties");

					b.Navigation("UserClaims");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.Client", b =>
				{
					b.Navigation("AllowedCorsOrigins");

					b.Navigation("AllowedGrantTypes");

					b.Navigation("AllowedScopes");

					b.Navigation("Claims");

					b.Navigation("ClientSecrets");

					b.Navigation("IdentityProviderRestrictions");

					b.Navigation("PostLogoutRedirectUris");

					b.Navigation("Properties");

					b.Navigation("RedirectUris");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", b =>
				{
					b.Navigation("Properties");

					b.Navigation("UserClaims");
				});
#pragma warning restore 612, 618
		}
	}
}
