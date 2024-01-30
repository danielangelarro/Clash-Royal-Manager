using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ClashRoyalManager.Domain.Entities;

namespace ClashRoyalManager.Application.Entities;

public class User : IUser
{
    [Key]
    public Guid ID_User { get; set; }
    public string? Photo { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? Correo { get; set; }
    public string? Password { get; set; } = null!;
    public string? Rol { get; set; } = null!;

    // Relaciones
    public ICollection<Entrada> Entradas { get; set; }
    public ICollection<Salida> Salidas { get; set; }
}