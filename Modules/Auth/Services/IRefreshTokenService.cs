namespace Backend.GestorProyectos.Services
{
    public interface IRefreshTokenService
    {
        Task<AuthResponse?> LoginAsync(string email, string password);
        Task<AuthResponse?> RefreshTokenAsync(string refreshToken);
        Task<string> GenerateRefreshTokenAsync(Usuario usuario); // ← Asegúrate de que esté esta línea
    }
}