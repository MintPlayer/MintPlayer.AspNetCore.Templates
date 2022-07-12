namespace MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

public class TwoFactorCode
{
    public string Code { get; set; }
    public Enums.ECodeType Type { get; set; }
}
