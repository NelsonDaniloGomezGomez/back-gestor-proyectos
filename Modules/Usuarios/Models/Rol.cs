namespace Backend.GestorProyectos.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        public string Descripcion { get; set; } = null!;
        
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}