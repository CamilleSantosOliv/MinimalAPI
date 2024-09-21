using ApiCatalogoMinimalAPI.Context;
using ApiCatalogoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoMinimalAPI.ApiEndpoints
{
    public static class ProdutosEndpoints
    {

        public static void MapProdutosEndpoints(this WebApplication app)
        {
            app.MapPost("/IncluirProduto", async (Produto produto, AppDbContext db)
    => {
        db.Produtos.Add(produto);
        await db.SaveChangesAsync();

        return Results.Created($"/produtos/{produto.ProdutoId}", produto);
    });

            app.MapGet("/ConsultarProdutos", async (AppDbContext db) => {
                var produtos = await db.Produtos.ToListAsync();
                return Results.Ok(produtos);
            }).WithTags("Produtos").RequireAuthorization();

            app.MapGet("/ConsultarProdutoPorId", async (int id, AppDbContext db) => {
                var produto = await db.Produtos.FindAsync(id);
                return produto != null ? Results.Ok(produto)
                                         : Results.NotFound($"Não foi encontrado na tabela o ID de número {id}.");
            });

            app.MapPut("/AlterarProduto/{id:int}", async (int id, Produto produto, AppDbContext db) => {
                if (produto.ProdutoId != id) {
                    return Results.BadRequest();
                }

                var produtoDB = await db.Produtos.FindAsync(id);

                if (produtoDB == null)
                    return Results.NotFound($"Não existe o ID {id} na base de dados.");

                produtoDB.DataCompra = produto.DataCompra;
                produtoDB.Imagem = produto.Imagem;
                produtoDB.Descricao = produto.Descricao;
                produtoDB.Estoque = produto.Estoque;
                produtoDB.Preco = produto.Preco;
                produtoDB.CategoriaId = produto.CategoriaId;
                produtoDB.Nome = produtoDB.Nome;

                await db.SaveChangesAsync();

                return Results.Ok(produtoDB);
            });

            app.MapDelete("/DeletarProduto/{id:int}", async (int id, AppDbContext db) => {

                var produtoDB = await db.Produtos.FindAsync(id);

                if (produtoDB == null) return Results.NotFound($"O ID {id} não foi encontrado na base de dados.");

                db.Produtos.Remove(produtoDB);
                db.SaveChanges();

                return Results.NoContent();
            });

        }
    }
}
