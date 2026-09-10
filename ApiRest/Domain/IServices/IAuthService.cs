namespace ApiRest.Domain.IServices;

using ApiRest.Domain.Models;
using ApiRest.DTO;

public interface IAuthService
{
    Task<AuthResponse> RegistrarUsuarioAsync(RegistroRequest request);
    Task<AuthResponse> AutenticarUsuarioAsync(LoginRequest request);
    Task<UsuarioResponse> ObtenerPerfilAsync(int id_usuario);
}