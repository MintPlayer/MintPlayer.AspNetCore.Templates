namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Options;

public class SmtpOptions
{
	public string SmtpHost { get; set; }
	public int SmtpPort { get; set; }
	public bool UseTLS { get; set; }
	public string SmtpUsername { get; set; }
	public string SmtpPassword { get; set; }
	public string DefaultFrom { get; set; }
}
