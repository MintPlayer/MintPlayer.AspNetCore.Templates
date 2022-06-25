using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Persistance.Entities;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Persistance;

internal class MeteoContext : IdentityDbContext<User, Role, Guid>
{
}
