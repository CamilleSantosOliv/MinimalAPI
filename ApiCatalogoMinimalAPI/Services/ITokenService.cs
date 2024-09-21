using ApiCatalogoMinimalAPI.Models;

namespace ApiCatalogoMinimalAPI.Services
{
    public interface ITokenService
    {
        string GerarToken(string key, string issuer, string audience, UserModel user);
    }
}
