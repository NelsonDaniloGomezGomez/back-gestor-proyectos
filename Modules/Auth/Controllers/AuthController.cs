[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _authService.ValidarUsuarioAsync(request.Email, request.Password);

        if (usuario == null)
            return Unauthorized(new { mensaje = "Credenciales inválidas" });

        var token = _authService.GenerarToken(usuario);
        
        return Ok(new
        {
            message = "Logueado correctamente",
            token = token
        });
    }
}
