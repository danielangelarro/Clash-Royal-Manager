using System.ComponentModel.DataAnnotations;

namespace ClashRoyalManager.Domain.Entities;

public class IUser
{
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? Correo { get; set; }
}