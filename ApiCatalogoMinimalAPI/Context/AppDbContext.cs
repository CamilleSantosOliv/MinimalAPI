using ApiCatalogoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoMinimalAPI.Context
{
    public class AppDbContext : DbContext
    {
        //classe que vai gerenciar o banco de dados MySQL
        //esta classe que interage e preenche o banco de dados 

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //passa as configuracoes da classe DbContextOptions para a classe base (dbcontext)
        }

        //inclui uma referencia de categoria e produto no banco de dados
        //as tabelas que serao criadas:
        public DbSet<Produto>? Produtos { get; set; }
        public DbSet<Categoria>? Categorias { get; set; }

        //esse metodo vai ser chamado quando o contexto for construido
        //define como a model (tipos dos campos) vao ser recebidos 
        //define uma configuracao para as propriedades 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //definindo a chave primaria como CategoriaId para a tabela de categoria
            modelBuilder.Entity<Categoria>().HasKey(c => c.CategoriaId);

            modelBuilder.Entity<Categoria>().Property(e => e.Nome).HasMaxLength(100).IsRequired();

            modelBuilder.Entity<Categoria>().Property(e => e.Descricao).HasMaxLength(150).IsRequired();

            //configuracoes para Produtos
            modelBuilder.Entity<Produto>().HasKey(c => c.ProdutoId);

            modelBuilder.Entity<Produto>().Property(e => e.Nome).HasMaxLength(100).IsRequired();

            modelBuilder.Entity<Produto>().Property(e => e.Descricao).HasMaxLength(150).IsRequired();

            modelBuilder.Entity<Produto>().Property(e => e.Imagem).HasMaxLength(100).IsRequired();

            //14 digitos com 2 casas decimais 
            modelBuilder.Entity<Produto>().Property(e => e.Preco).HasPrecision(14, 2);


            //relacionamento
            //definindo a chave estrangeira 
            modelBuilder.Entity<Produto>()
                .HasOne<Categoria>(c => c.Categoria)
                .WithMany(p => p.Produtos).HasForeignKey(c => c.CategoriaId);

        }
    }
}
