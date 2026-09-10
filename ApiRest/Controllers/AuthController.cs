namespace ApiRest.Controllers;

using Microsoft.AspNetCore.Mvc;
using ApiRest.Domain.IServices;
using ApiRest.DTO;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registrar")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registrar([FromBody] RegistroRequest request)
    {
        try
        {
            var resultado = await _authService.RegistrarUsuarioAsync(request);
            return CreatedAtAction(nameof(ObtenerPerfil), new { id = resultado.Id }, resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
}
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno del servidor" });
            }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var resultado = await _authService.AutenticarUsuarioAsync(request);
            return Ok(resultado);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { mensaje = "Credenciales inválidas" });
}
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno del servidor" });
            }
    }

    [HttpGet("perfil/{id:int}")]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPerfil(int id)
    {
        try
        {
            var perfil = await _authService.ObtenerPerfilAsync(id);
            return Ok(perfil);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
}
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno del servidor" });
            }
    }

    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult HealthCheck()
    {
        return Ok(new { status = "Servicio de autenticación funcionando", timestamp = DateTime.UtcNow });
    }
}