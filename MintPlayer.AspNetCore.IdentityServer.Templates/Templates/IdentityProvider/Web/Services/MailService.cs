using Microsoft.Extensions.Options;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Services;
using System.Net.Mail;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Services;

internal class MailService : IMailService
{
	#region Constructor
	private readonly IOptions<Data.Options.SmtpOptions> smtpOptions;
	public MailService(IOptions<Data.Options.SmtpOptions> smtpOptions)
	{
		this.smtpOptions = smtpOptions;
	}
	#endregion

	public Task<SmtpClient> CreateSmtpClient()
	{
		var client = new SmtpClient
		{
			Host = smtpOptions.Value.SmtpHost,
			Port = smtpOptions.Value.SmtpPort,
			EnableSsl = smtpOptions.Value.UseTLS,
			DeliveryMethod = SmtpDeliveryMethod.Network,
			Credentials = new System.Net.NetworkCredential(smtpOptions.Value.SmtpUsername, smtpOptions.Value.SmtpPassword),
		};

		return Task.FromResult(client);
	}
}
