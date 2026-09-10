namespace ApiRest.DTO;

using System.Text.Json.Serialization;

public class AuthResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [JsonPropertyName("apellido")]
    public string Apellido { get; set; } = string.Empty;

    [JsonPropertyName("correo_electronico")]
    public string CorreoElectronico { get; set; } = string.Empty;

    [JsonPropertyName("rol")]
    public string Rol { get; set; } = string.Empty;

    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyName("fecha_expiracion")]
    public DateTime FechaExpiracion { get; set; }
}