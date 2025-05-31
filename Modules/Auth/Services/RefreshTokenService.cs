namespace Backend.GestorProyectos.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly GestorProyectosDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RefreshTokenService(GestorProyectosDbContext context, JwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponse?> RefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(t => t.Token == refreshToken && t.Expires > DateTime.UtcNow);

            if (token == null)
                return null;

            var accessToken = _jwtService.GenerarToken(token.Usuario);

            // Usamos el token existente para la expiración
            var newRefreshToken = await GenerateRefreshTokenAsync(token.Usuario);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                Expires = DateTime.UtcNow.AddDays(7) // Aquí asumimos que el nuevo token expira en 7 días
            };
        }

        public async Task<string> GenerateRefreshTokenAsync(Usuario usuario)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(randomBytes);

            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var token = new RefreshToken
            {
                Token = refreshToken,
                UsuarioId = usuario.Id,
                Expires = DateTime.UtcNow.AddDays(3),
                CreatedByIp = ipAddress,
                Created = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<AuthResponse?> LoginAsync(string email, string password)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new UnauthorizedAccessException();

            var passwordHasher = new PasswordHasher<Usuario>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException();

            var accessToken = _jwtService.GenerarToken(user);
            var refreshToken = await GenerateRefreshTokenAsync(user);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7) // Aquí ponemos la expiración del refresh token
            };
        }
    }
}
