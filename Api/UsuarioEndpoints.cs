namespace Backend.GestorProyectos.Endpoints
{
    public static class UsuarioEndpoints
    {
        public static void MapUsuarioEndpoints(this WebApplication app)
        {
            app.MapGet("/usuarios", [Authorize] (UsuarioService service) =>
            {
                return service.ObtenerUsuarios();
            }).WithName("GetUsuarios");

            app.MapGet("/usuarios/{id:int}", (int id, UsuarioService service) =>
            {
                try
                {
                    var usuario = service.ObtenerUsuarioPorId(id);
                    return Results.Ok(usuario);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { mensaje = ex.Message });
                }
                catch (Exception)
                {
                    // Puedes loguear el error aquí si tienes logger
                    return Results.Problem("Error interno del servidor");
                }
            }).WithName("GetUsuarioById");

            app.MapPost("/usuarios", (Usuario usuario, UsuarioService service) =>
            {
                service.CrearUsuario(usuario);
                return Results.Created($"/usuarios/{usuario.Id}", usuario);
            }).WithName("CrearUsuario");

            app.MapPut("/usuarios/{id:int}", (int id, Usuario usuario, UsuarioService service) =>
            {
                var existente = service.ObtenerUsuarioPorId(id);
                if (existente == null) return Results.NotFound();

                usuario.Id = id;
                service.ActualizarUsuario(usuario);
                return Results.NoContent();
            }).WithName("ActualizarUsuario");

            app.MapDelete("/usuarios/{id:int}", (int id, UsuarioService service) =>
            {
                var existente = service.ObtenerUsuarioPorId(id);
                if (existente == null) return Results.NotFound();

                service.EliminarUsuario(id);
                return Results.NoContent();
            }).WithName("EliminarUsuario");

            app.MapGet("/api/usuario/me", [Authorize] (HttpContext httpContext) =>
            {
                var id = httpContext.User.FindFirst("id")?.Value;
                var nombre = httpContext.User.FindFirst("nombre")?.Value;
                var email = httpContext.User.FindFirst(ClaimTypes.Name) ?? httpContext.User.FindFirst("sub");

                return Results.Ok(new
                {
                    Id = id,
                    Nombre = nombre,
                    Email = email?.Value
                });
            });
        }
    }
}
