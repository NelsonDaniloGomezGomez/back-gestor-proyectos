public class AuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Usuario? ValidarUsuario(string email, string password)
    {
        // Aquí debes reemplazar con tu lógica de validación real
        if (email == "nelsondanilogomezgomez@gmail.com" && password == "123456")
        {
            return new Usuario
            {
                Id = 1,
                Email = email,
                Nombre = "Admin"
            };
        }

        return null;
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
