namespace Backend.GestorProyectos.Dtos.Usuario
{
    public class UsuarioRegisterDto
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        [Required]
        public int IdRolCreador { get; set; }

        [Required]
        public int IdRol { get; set; }
    }
}