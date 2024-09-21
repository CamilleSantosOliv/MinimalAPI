using ApiCatalogoMinimalAPI.ApiEndpoints;
using ApiCatalogoMinimalAPI.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

//add confg Swagger
builder.AddApiSwagger();

builder.AppPersistence();

//habilitacao dos servicos Cors
builder.Services.AddCors();

//autenticação JWT
builder.AddAutentication(); 

var app = builder.Build();

//Endpoint para login
app.MapAutenticacaoEndpoints();

//Endpoints de Categoria
app.MapCategoriasEndpoints();

//Endpoints de Produto
app.MapProdutosEndpoints();

//metodos de extensão
var environment = app.Environment;
app.UserExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
