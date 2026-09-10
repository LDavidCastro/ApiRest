namespace ApiRest.DTO;

using System.Text.Json.Serialization;

public class RegistroRequest
{
    [JsonPropertyName("nombre_completo")]
    public string NombreCompleto { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("rol")]
    public string Rol { get; set; } = "usuario";
}