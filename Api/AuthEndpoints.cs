namespace Backend.GestorProyectos.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/login", (LoginRequest loginRequest, AuthService authService) =>
            {
                var usuario = authService.ValidarUsuario(loginRequest.Email, loginRequest.Password);
                if (usuario == null)
                    return Results.Json(new { mensaje = "Credenciales inválidas" }, statusCode: 401);

                
                var token = authService.GenerarToken(usuario);
                return Results.Ok(new { token });
            });

            app.MapGet("/api/protected", [Authorize] () =>
            {
                return Results.Ok(new { mensaje = "Acceso autorizado. ¡Bienvenido!" });
            });
        }
    }
}
