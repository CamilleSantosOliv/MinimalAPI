using ApiCatalogoMinimalAPI.Context;
using ApiCatalogoMinimalAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiCatalogoMinimalAPI.AppServicesExtensions
{
    public static class ServicecollectionsExtensions
    {
        public static WebApplicationBuilder AddApiSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwagger();
            return builder;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
                    Title = "ApiCatalogo",
                    Version = "v1"
                });

                // Definindo a segurança usando o esquema Bearer JWT
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme() {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = @"JWT Authorization header using the Bearer scheme.
                        Enter 'Bearer' [space] and then your token. 
                        Example: 'Bearer 12345abcdef'",
                });

                // Definindo o requisito de segurança para o esquema Bearer
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            return services;
        }

        public static WebApplicationBuilder AppPersistence(this WebApplicationBuilder builder)
        {
            // Carregar as variáveis de ambiente
            builder.Configuration.AddEnvironmentVariables();

            string connectionString = builder.Configuration["ConnectionStringEnvironmentVariable"]
        ?? throw new Exception("Connection string is not configured.");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddSingleton<ITokenService>(new TokenService());

            return builder;
        }


        public static WebApplicationBuilder AddAutentication(this WebApplicationBuilder builder)
        {
            string jwtKey = Environment.GetEnvironmentVariable("JwtKeyEnvironmentVariable", EnvironmentVariableTarget.Machine);

            if (string.IsNullOrEmpty(jwtKey)) {
                throw new Exception("A chave JWT não está configurada ou não foi recuperada corretamente.");
            }

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options => {
                   options.TokenValidationParameters = new TokenValidationParameters {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = builder.Configuration["Jwt:Issuer"],  // Certifique-se de que Jwt:Issuer está correto
                       ValidAudience = builder.Configuration["Jwt:Audience"], // Verifique se Jwt:Audience está corretamente definido
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) // Usando a chave JWT
                   };
               });

            builder.Services.AddAuthorization();

            return builder;
        }
    }
}
