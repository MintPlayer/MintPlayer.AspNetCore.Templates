using System.Net.Mail;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Services;

public interface IMailService
{
	Task<SmtpClient> CreateSmtpClient();
}