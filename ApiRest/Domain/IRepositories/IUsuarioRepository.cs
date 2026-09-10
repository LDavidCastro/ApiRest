namespace ApiRest.Domain.IRepositories;

using ApiRest.Domain.Models;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorIdAsync(int id_usuario);
    Task<Usuario?> ObtenerPorEmailAsync(string email);
    Task<IEnumerable<Usuario>> ObtenerTodosAsync();
    Task<Usuario> CrearAsync(Usuario usuario);
    Task<Usuario> ActualizarAsync(Usuario usuario);
    Task<bool> EliminarAsync(int id_usuario);
    Task<bool> ExisteEmailAsync(string email);
}