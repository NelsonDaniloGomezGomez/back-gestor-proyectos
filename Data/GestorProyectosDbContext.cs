using Microsoft.EntityFrameworkCore;
using Backend.GestorProyectos.Models;

public class GestorProyectosDbContext : DbContext
{
    public GestorProyectosDbContext(DbContextOptions<GestorProyectosDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
}
