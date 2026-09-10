namespace ApiRest.DTO;

using System.Text.Json.Serialization;

public class RegistroRequest
{
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [JsonPropertyName("apellido")]
    public string Apellido { get; set; } = string.Empty;

    [JsonPropertyName("correo_electronico")]
    public string CorreoElectronico { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("rol")]
    public string Rol { get; set; } = "usuario";
}