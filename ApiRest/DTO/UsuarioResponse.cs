namespace ApiRest.DTO;

using System.Text.Json.Serialization;

public class UsuarioResponse
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

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fecha_creacion")]
    public DateTime FechaCreacion { get; set; }

    [JsonPropertyName("ultimo_acceso")]
    public DateTime? UltimoAcceso { get; set; }
}