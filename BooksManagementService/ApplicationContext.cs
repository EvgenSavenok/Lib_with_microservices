using BooksManagementService.Models;
using Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookConfiguration());
    }
    public DbSet<Book> Books { get; set; }
}
