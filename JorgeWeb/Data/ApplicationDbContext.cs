using JorgeWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace JorgeWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // consola add-migration AddCategoryTableToDb
        // Agrega la tabla a la base de datos (desde el modelo)
        public DbSet<Category> Catagories { get; set; }

        //Consola add-migration SeedCatagoryTable
        //agrega datos a la tabla ya creada
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category  { Id = 1, Name = "action", DisplayOrder = 1 },
                new Category  { Id = 2, Name = "sci", DisplayOrder = 2 },
                new Category  { Id = 3, Name = "History", DisplayOrder = 3 }
             );
        }
    }
}
