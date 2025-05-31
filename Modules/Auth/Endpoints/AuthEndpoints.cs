namespace Backend.GestorProyectos.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/login", async (
                LoginRequest loginRequest,
                AuthService authService,
                IRefreshTokenService refreshTokenService) =>
            {
                var usuario = await authService.ValidarUsuarioAsync(loginRequest.Email, loginRequest.Password);
                if (usuario == null)
                    return Results.Json(new { mensaje = "Credenciales inválidas" }, statusCode: 401);

                var accessToken = authService.GenerarToken(usuario);
                var refreshToken = await refreshTokenService.GenerateRefreshTokenAsync(usuario);

                return Results.Ok(new AuthResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            });

            app.MapPost("/refresh-token", async (RefreshTokenRequest request, IRefreshTokenService refreshTokenService) =>
            {
                var result = await refreshTokenService.RefreshTokenAsync(request.RefreshToken);
                if (result is null)
                    return Results.Unauthorized();

                return Results.Ok(result);
            });

            app.MapGet("/api/protected", [Authorize] () =>
            {
                return Results.Ok(new { mensaje = "Acceso autorizado. ¡Bienvenido!" });
            });
        }
    }
}
