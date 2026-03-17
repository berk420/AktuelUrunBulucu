using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AktuelUrunBulucu.DAL.Entities;

[Table("user_coordinates")]
public class UserCoordinate
{
    [Column("id")]
    public int Id { get; set; }

    [Column("ip_address")]
    [Required]
    public string IpAddress { get; set; } = string.Empty;

    [Column("latitude")]
    public double Latitude { get; set; }

    [Column("longitude")]
    public double Longitude { get; set; }

    [Column("saved_at")]
    public DateTime SavedAt { get; set; }
}
