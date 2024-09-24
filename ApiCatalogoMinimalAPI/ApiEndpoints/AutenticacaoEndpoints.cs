using ApiCatalogoMinimalAPI.Models;
using ApiCatalogoMinimalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;

namespace ApiCatalogoMinimalAPI.ApiEndpoints
{
    public static class AutenticacaoEndpoints
    {
        public static void MapAutenticacaoEndpoints(this WebApplication app)
        {
            app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) => {

                if (userModel == null) {
                    return Results.BadRequest("Login inválido.");
                }

                if (userModel.UserName == "camille" && userModel.Password == "teste") {

                    // Recuperando a chave JWT do ambiente
                    string key = Environment.GetEnvironmentVariable("JwtKeyEnvironmentVariable", EnvironmentVariableTarget.Machine);
                    string issuer = app.Configuration["Jwt:Issuer"];
                    string audience = app.Configuration["Jwt:Audience"];

                    // Verifique se esses valores não são nulos
                    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience)) {
                        return Results.BadRequest("Configurações do JWT estão ausentes.");
                    }

                    var tokenString = tokenService.GerarToken(key, issuer, audience, userModel);

                    return Results.Ok(new { token = tokenString });
                }
                else {
                    return Results.BadRequest("Login inválido.");
                }
            })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("Login")
            .WithTags("Autenticacao");
        }
    }

}
