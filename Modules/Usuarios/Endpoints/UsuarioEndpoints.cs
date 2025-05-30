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

            app.MapPost("/register", [Authorize] async (
                UsuarioRegisterDto request,
                HttpContext http,
                UsuarioService usuarioService) =>
            {
                try
                {
                    // Extraer el Id del creador desde los claims del token (asegúrate que se envíe en el JWT)
                    var creadorIdString = http.User.FindFirst("id")?.Value;
                    if (creadorIdString == null)
                        return Results.Unauthorized();

                    int creadorId = int.Parse(creadorIdString);

                    // Obtener el rol del creador para lógica condicional (opcional, puedes delegar al servicio)
                    int rolCreadorId = int.TryParse(http.User.FindFirst("RolId")?.Value, out var rol) ? rol : 0;

                    bool esAdmin = rolCreadorId == (int)RoleEnum.Administrador;
                    bool esJefeODesarrollador = rolCreadorId == (int)RoleEnum.JefeDeProyecto || rolCreadorId == (int)RoleEnum.Desarrollador;

                    if (!esAdmin && !esJefeODesarrollador)
                    {
                        return Results.Json(new { mensaje = "No tiene permisos para registrar nuevos usuarios", rolCreadorId }, statusCode: 403);
                    }

                    var usuario = new Usuario
                    {
                        Nombre = request.Nombre,
                        Email = request.Email,
                        // Solo admins pueden asignar rol, otros siempre Invitado
                        RolId = esAdmin ? request.IdRol : (int)RoleEnum.Invitado
                    };

                    var nuevoUsuario = await usuarioService.CrearUsuarioAsync(usuario, request.Password, creadorId);
                    return Results.Created($"/usuarios/{nuevoUsuario.Id}", nuevoUsuario);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { mensaje = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { mensaje = ex.Message });
                }
            });

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
