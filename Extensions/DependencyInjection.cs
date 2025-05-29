// Backend.GestorProyectos/Extensions/DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;
using Backend.GestorProyectos.Services;

namespace Backend.GestorProyectos.Extensions
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
