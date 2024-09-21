using ApiCatalogoMinimalAPI.Context;
using ApiCatalogoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoMinimalAPI.ApiEndpoints
{
    public static class CategoriasEndpoints
    {
        public static void MapCategoriasEndpoints(this WebApplication app)
        {
            app.MapPost("/IncluirCategoria", async (Categoria categoria, AppDbContext db)
                => {
                    db.Categorias.Add(categoria);
                    await db.SaveChangesAsync();

                    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
                });

            app.MapGet("/ConsultarCategorias", async (AppDbContext db) => {
                var categorias = await db.Categorias.ToListAsync();
                return Results.Ok(categorias); //se algo deve aparecer na tela, deve-se retornar o resultado sempre pro usuario
            }).WithTags("Categorias").RequireAuthorization();

            app.MapGet("/ConsultarCategoriaPorId", async (int id, AppDbContext db) => {
                var categoria = await db.Categorias.FindAsync(id);
                return categoria != null ? Results.Ok(categoria)
                                         : Results.NotFound($"Não foi encontrado na tabela o ID de número {id}.");
            });

            app.MapPut("/AlterarCategoria/{id:int}", async (int id, Categoria categoria, AppDbContext db) => {
                if (categoria.CategoriaId != id) {
                    return Results.BadRequest();
                }

                var categoriaDB = await db.Categorias.FindAsync(id);

                if (categoriaDB == null)
                    return Results.NotFound($"Não existe o ID {id} na base de dados.");

                categoriaDB.Descricao = categoria.Descricao;
                categoriaDB.Nome = categoria.Nome;

                await db.SaveChangesAsync();

                return Results.Ok(categoriaDB);
            });

            app.MapDelete("/DeletarCategoria/{id:int}", async (int id, AppDbContext db) => {

                var categoriaDB = await db.Categorias.FindAsync(id);

                if (categoriaDB == null) return Results.NotFound($"O ID {id} não foi encontrado na base de dados.");

                db.Categorias.Remove(categoriaDB);
                db.SaveChanges();

                return Results.NoContent();
            });

        }
    }
}
