using Microsoft.AspNetCore.Identity;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Persistance.Entities;

internal class User : IdentityUser<Guid>
{
	public bool Bypass2faForExternalLogin { get; set; }
}
