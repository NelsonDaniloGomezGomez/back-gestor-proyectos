namespace Backend.GestorProyectos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(JwtService jwtService, IRefreshTokenService refreshTokenService)
        {
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var result = await _refreshTokenService.LoginAsync(request.Email, request.Password);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas" });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _refreshTokenService.RefreshTokenAsync(refreshToken);
            if (result == null) return Unauthorized();
            return Ok(result);
        }
    }
}