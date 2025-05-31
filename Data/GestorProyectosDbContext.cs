public class GestorProyectosDbContext : DbContext
{
    public GestorProyectosDbContext(DbContextOptions<GestorProyectosDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }  // <- Agregado

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Datos iniciales para Roles
        modelBuilder.Entity<Rol>().HasData(
            new Rol { Id = 1, Nombre = "Administrador", Descripcion = "Control total del sistema: gestión de usuarios, roles y configuración general." },
            new Rol { Id = 2, Nombre = "Jefe de Proyecto", Descripcion = "Responsable de crear, asignar y supervisar proyectos y tareas." },
            new Rol { Id = 3, Nombre = "Desarrollador", Descripcion = "Realiza tareas asignadas, actualiza estados y reporta avances." },
            new Rol { Id = 4, Nombre = "Tester/QA", Descripcion = "Encargado de probar funcionalidades, reportar errores y validar entregables." },
            new Rol { Id = 5, Nombre = "Cliente", Descripcion = "Usuario externo con permisos limitados para ver avances y reportes." },
            new Rol { Id = 6, Nombre = "Invitado", Descripcion = "Acceso muy limitado o solo lectura a ciertas partes del sistema." }
        );
    }
}