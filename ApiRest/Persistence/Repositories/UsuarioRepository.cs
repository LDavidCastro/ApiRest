namespace ApiRest.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using ApiRest.Domain.Models;
using ApiRest.Domain.IRepositories;
using ApiRest.Persistence.Context;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.id == id);
    }

    public async Task<Usuario?> ObtenerPorEmailAsync(string correo_electronico)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.correo_electronico == correo_electronico && u.activo);
    }

    public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
    {
        return await _context.Usuarios
            .Where(u => u.activo)
            .ToListAsync();
    }

    public async Task<Usuario> CrearAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario> ActualizarAsync(Usuario usuario)
    {
        usuario.fecha_actualizacion = DateTime.UtcNow;
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.id == id);

        if (usuario == null)
            return false;

        usuario.activo = false;
        usuario.fecha_actualizacion = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExisteEmailAsync(string correo_electronico)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.correo_electronico == correo_electronico && u.activo);
    }
}