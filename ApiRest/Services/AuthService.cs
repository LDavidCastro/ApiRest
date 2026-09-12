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
        if (await _usuarioRepository.ExisteEmailAsync(request.CorreoElectronico))
        {
            throw new ArgumentException("El email ya está registrado");
        }

        var usuario = new Usuario
        {
            nombre = request.Nombre,
            apellido = request.Apellido,
            correo_electronico = request.CorreoElectronico,
            contrasena_hash = BCrypt.HashPassword(request.Password),
            rol = request.Rol
        };

        var usuarioCreado = await _usuarioRepository.CrearAsync(usuario);
        var token = GenerarToken(usuarioCreado);

        return new AuthResponse
        {
            Id = usuarioCreado.id,
            Nombre = usuarioCreado.nombre,
            Apellido = usuarioCreado.apellido,
            CorreoElectronico = usuarioCreado.correo_electronico,
            Rol = usuarioCreado.rol,
            Token = token,
            FechaExpiracion = DateTime.UtcNow.AddHours(24)
        };
    }

    public async Task<AuthResponse> AutenticarUsuarioAsync(LoginRequest request)
    {
        Console.WriteLine($"DEBUG: Autenticando usuario con email: {request.CorreoElectronico}");

        var usuario = await _usuarioRepository.ObtenerPorEmailAsync(request.CorreoElectronico);

        if (usuario == null)
        {
            Console.WriteLine($"DEBUG: Usuario no encontrado con email: {request.CorreoElectronico}");
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        Console.WriteLine($"DEBUG: Usuario encontrado: ID={usuario.id}, Email={usuario.correo_electronico}");
        Console.WriteLine($"DEBUG: Hash almacenado: {usuario.contrasena_hash}");

        if (!BCrypt.Verify(request.Password, usuario.contrasena_hash))
        {
            Console.WriteLine($"DEBUG: Contraseña incorrecta para usuario: {usuario.id}");
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        Console.WriteLine($"DEBUG: Autenticación exitosa para usuario: {usuario.id}");
        usuario.ultimo_acceso = DateTime.UtcNow;
        await _usuarioRepository.ActualizarAsync(usuario);

        var token = GenerarToken(usuario);

        return new AuthResponse
        {
            Id = usuario.id,
            Nombre = usuario.nombre,
            Apellido = usuario.apellido,
            CorreoElectronico = usuario.correo_electronico,
            Rol = usuario.rol,
            Token = token,
            FechaExpiracion = DateTime.UtcNow.AddHours(24)
        };
    }

    public async Task<UsuarioResponse> ObtenerPerfilAsync(int id)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new KeyNotFoundException("Usuario no encontrado");
        }

        return new UsuarioResponse
        {
            Id = usuario.id,
            Nombre = usuario.nombre,
            Apellido = usuario.apellido,
            CorreoElectronico = usuario.correo_electronico,
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
            new Claim(ClaimTypes.NameIdentifier, usuario.id.ToString()),
            new Claim(ClaimTypes.Email, usuario.correo_electronico),
            new Claim(ClaimTypes.Name, usuario.nombre + " " + usuario.apellido),
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