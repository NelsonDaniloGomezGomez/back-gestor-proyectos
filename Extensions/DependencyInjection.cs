namespace Backend.GestorProyectos.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // registrar todos tus servicios, repositorios, etc.
            services.AddScoped<UsuarioService>();
            // services.AddScoped<OtroServicio>();
            // services.AddScoped<IRepository, Repository>();

            return services;
        }
    }
}
