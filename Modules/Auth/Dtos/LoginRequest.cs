public class LoginRequest
{

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string  Password { get; set; } = null!;
}