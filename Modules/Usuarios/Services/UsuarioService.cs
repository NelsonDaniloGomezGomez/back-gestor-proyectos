using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.GestorProyectos.Services
{
    public class UsuarioService
    {
        private readonly GestorProyectosDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher = new();

        public UsuarioService(GestorProyectosDbContext context)
        {
            _context = context;
        }

        // Método para registrar (crear) un usuario con hash de contraseña
        public async Task<Usuario> CrearUsuarioAsync(Usuario nuevoUsuario, string passwordSinHash, int creadorId)
        {
            var creador = await _context.Usuarios.FindAsync(creadorId);
            if (creador == null)
                throw new Exception("Creador no encontrado.");

            var rolAsignadoExiste = await _context.Roles.AnyAsync(r => r.Id == nuevoUsuario.RolId);
            if (!rolAsignadoExiste)
                throw new Exception("Rol asignado no es válido.");

            // Validar permisos del creador para asignar roles
            if (creador.RolId == (int)RoleEnum.Administrador)
            {
                // Admin puede asignar cualquier rol (ya está validado)
            }
            else if (creador.RolId == (int)RoleEnum.JefeDeProyecto || creador.RolId == (int)RoleEnum.Desarrollador)
            {
                // Solo pueden asignar rol Invitado
                nuevoUsuario.RolId = (int)RoleEnum.Invitado;
            }
            else
            {
                throw new Exception("No tienes permisos para crear usuarios.");
            }

            // Validar si el correo ya está registrado
            bool emailExiste = await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == nuevoUsuario.Email.ToLower());
            if (emailExiste)
                throw new Exception("El correo electrónico ya está en uso.");

            // Hashear la contraseña antes de guardar
            nuevoUsuario.Password = HashPassword(passwordSinHash);
            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return nuevoUsuario;
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return _context.Usuarios.ToList();
        }

        private string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
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
