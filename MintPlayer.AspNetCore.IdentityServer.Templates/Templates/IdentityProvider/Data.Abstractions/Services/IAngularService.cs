using Microsoft.AspNetCore.Mvc;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Services;

public interface IAngularService
{
	Task<string?> GetStylesheetUrl(IUrlHelper urlHelper);
}
