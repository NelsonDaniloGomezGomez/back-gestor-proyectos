public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly GestorProyectosDbContext _context;
    private readonly PasswordHasher<Usuario> _passwordHasher = new();

    public AuthService(IConfiguration configuration, GestorProyectosDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<Usuario?> ValidarUsuarioAsync(string email, string password)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        if (usuario == null)
            return null;

        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, password);

        return resultado == PasswordVerificationResult.Success ? usuario : null;
    }

    public string GenerarToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("id", usuario.Id.ToString()),
            new Claim("nombre", usuario.Nombre),
            new Claim("RolId", usuario.RolId.ToString()),
            new Claim(ClaimTypes.Name, usuario.Email)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}