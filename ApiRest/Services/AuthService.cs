namespace ApiRest.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ApiRest.Domain.Models;
using ApiRest.Domain.IRepositories;
using ApiRest.Domain.IServices;
using ApiRest.DTO;
using BCrypt.Net;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegistrarUsuarioAsync(RegistroRequest request)
    {
        if (await _usuarioRepository.ExisteEmailAsync(request.Email))
        {
            throw new ArgumentException("El email ya está registrado");
        }

        var usuario = new Usuario
        {
            nombre_completo = request.NombreCompleto,
            email = request.Email,
            password_hash = BCrypt.HashPassword(request.Password),
            rol = request.Rol
        };

        var usuarioCreado = await _usuarioRepository.CrearAsync(usuario);
        var token = GenerarToken(usuarioCreado);

        return new AuthResponse
        {
            IdUsuario = usuarioCreado.id_usuario,
            NombreCompleto = usuarioCreado.nombre_completo,
            Email = usuarioCreado.email,
            Rol = usuarioCreado.rol,
            Token = token,
            FechaExpiracion = DateTime.UtcNow.AddHours(24)
        };
    }

    public async Task<AuthResponse> AutenticarUsuarioAsync(LoginRequest request)
    {
        var usuario = await _usuarioRepository.ObtenerPorEmailAsync(request.Email);

        if (usuario == null || !BCrypt.Verify(request.Password, usuario.password_hash))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        usuario.ultimo_acceso = DateTime.UtcNow;
        await _usuarioRepository.ActualizarAsync(usuario);

        var token = GenerarToken(usuario);

        return new AuthResponse
        {
            IdUsuario = usuario.id_usuario,
            NombreCompleto = usuario.nombre_completo,
            Email = usuario.email,
            Rol = usuario.rol,
            Token = token,
            FechaExpiracion = DateTime.UtcNow.AddHours(24)
        };
    }

    public async Task<UsuarioResponse> ObtenerPerfilAsync(int id_usuario)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(id_usuario);

        if (usuario == null)
        {
            throw new KeyNotFoundException("Usuario no encontrado");
        }

        return new UsuarioResponse
        {
            IdUsuario = usuario.id_usuario,
            NombreCompleto = usuario.nombre_completo,
            Email = usuario.email,
            Rol = usuario.rol,
            Activo = usuario.activo,
            FechaCreacion = usuario.fecha_creacion,
            UltimoAcceso = usuario.ultimo_acceso
        };
    }

    private string GenerarToken(Usuario usuario)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? "mi_clave_secreta_muy_segura_y_larga_para_jwt_token_2024";
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "ApiRest";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "ApiRestClients";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.id_usuario.ToString()),
            new Claim(ClaimTypes.Email, usuario.email),
            new Claim(ClaimTypes.Name, usuario.nombre_completo),
            new Claim(ClaimTypes.Role, usuario.rol)
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}