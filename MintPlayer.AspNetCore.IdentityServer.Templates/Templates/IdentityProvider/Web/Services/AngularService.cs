using Microsoft.AspNetCore.Mvc;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Services;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Services;

internal class AngularService : IAngularService
{
	#region Constructor
	private readonly IWebHostEnvironment webHostEnvironment;
	private readonly IServiceProvider serviceProvider;
	public AngularService(IWebHostEnvironment webHostEnvironment, IServiceProvider serviceProvider)
	{
		this.webHostEnvironment = webHostEnvironment;
		this.serviceProvider = serviceProvider;
	}
	#endregion

	public Task<string?> GetStylesheetUrl(IUrlHelper urlHelper)
	{
		if (webHostEnvironment.IsDevelopment())
		{
			return Task.FromResult<string?>("/styles.css");
		}
		else
		{
			var root = webHostEnvironment.ContentRootPath + "/ClientApp/dist";
			var stylesheet = Directory.GetFiles(root, "styles.*.css", SearchOption.AllDirectories)
				.Select(f => Path.GetFileName(f))
				.Select(f => urlHelper.Content($"~/{f}"))
				.FirstOrDefault();
			return Task.FromResult(stylesheet);
		}
	}
}
