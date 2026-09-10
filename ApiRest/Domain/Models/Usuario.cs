namespace ApiRest.Domain.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }

    [Required]
    [StringLength(50)]
    public string nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string apellido { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string correo_electronico { get; set; } = string.Empty;

    [Required]
    [JsonIgnore]
    public string contrasena_hash { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string rol { get; set; } = "usuario";

    public bool activo { get; set; } = true;

    public DateTime fecha_creacion { get; set; } = DateTime.UtcNow;

    public DateTime? fecha_actualizacion { get; set; }

    public DateTime? ultimo_acceso { get; set; }
}