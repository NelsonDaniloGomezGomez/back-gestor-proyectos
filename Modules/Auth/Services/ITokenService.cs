namespace Backend.GestorProyectos.Services
{
public interface ITokenService
{
    Task<string> GenerateRefreshTokenAsync(Usuario usuario);
    Task<bool> ValidateRefreshTokenAsync(string token);
    Task<string?> GetUserIdFromRefreshTokenAsync(string token);
    Task InvalidateRefreshTokenAsync(string token);
}

}