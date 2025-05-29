namespace Backend.GestorProyectos.Services

{
    public class UsuarioService
    {
        private readonly GestorProyectosDbContext _context;

        public UsuarioService(GestorProyectosDbContext context)
        {
            _context = context;
        }

        public void CrearUsuario(Usuario usuario)
        {
            usuario.FechaCreacion = DateTime.Now;
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return _context.Usuarios.ToList();
        }

        public Usuario ObtenerUsuarioPorId(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);

            if (usuario == null)
            {
                throw new KeyNotFoundException($"No se encontró el usuario con Id {id}.");
            }
            return usuario;
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            var existente = _context.Usuarios.FirstOrDefault(u => u.Id == usuario.Id);
            if (existente == null)
            {
                throw new KeyNotFoundException($"Usuario con Id {usuario.Id} no encontrado.");
            }

            existente.Nombre = usuario.Nombre;
            existente.Email = usuario.Email;
            _context.SaveChanges();
        }

        public void EliminarUsuario(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                throw new KeyNotFoundException($"Usuario con Id {id} no encontrado.");
            }
            
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }
    }
}
