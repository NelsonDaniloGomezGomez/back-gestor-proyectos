namespace Backend.GestorProyectos.Services
{
    public class AuthService
    {
        private readonly GestorProyectosDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher = new();
        private readonly JwtService _jwtService;

        public AuthService(GestorProyectosDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
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
            return _jwtService.GenerarToken(usuario);
        }
    }
}