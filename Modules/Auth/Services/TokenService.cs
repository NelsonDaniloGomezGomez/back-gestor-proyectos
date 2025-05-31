namespace Backend.GestorProyectos.Services
{
public class TokenService : ITokenService
{
    private readonly GestorProyectosDbContext _context;

    public TokenService(GestorProyectosDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateRefreshTokenAsync(Usuario usuario)
    {
        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            Expires = DateTime.UtcNow.AddDays(7),
            UsuarioId = usuario.Id
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken.Token;
    }

    public async Task<bool> ValidateRefreshTokenAsync(string token)
    {
        var existingToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token && rt.Expires > DateTime.UtcNow);

        return existingToken != null;
    }

    public async Task<string?> GetUserIdFromRefreshTokenAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);

        return refreshToken?.UsuarioId.ToString();
    }

    public async Task InvalidateRefreshTokenAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);

        if (refreshToken != null)
        {
            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}

}