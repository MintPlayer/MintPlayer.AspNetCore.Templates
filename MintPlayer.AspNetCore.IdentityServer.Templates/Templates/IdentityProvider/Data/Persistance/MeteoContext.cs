using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance.Entities;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance;

internal class MeteoContext : IdentityDbContext<User, Role, Guid>
{
}
