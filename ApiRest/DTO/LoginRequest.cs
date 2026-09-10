namespace ApiRest.DTO;

using System.Text.Json.Serialization;

public class LoginRequest
{
    [JsonPropertyName("correo_electronico")]
    public string CorreoElectronico { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}