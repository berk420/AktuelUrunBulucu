using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AktuelUrunBulucu.DAL.Entities;

[Table("notification_requests")]
public class NotificationRequest
{
    [Column("id")]
    public int Id { get; set; }

    [Column("ip_address")]
    [Required]
    public string IpAddress { get; set; } = string.Empty;

    [Column("email")]
    [Required]
    public string Email { get; set; } = string.Empty;

    [Column("searched_product")]
    [Required]
    public string SearchedProduct { get; set; } = string.Empty;

    [Column("requested_at")]
    public DateTime RequestedAt { get; set; }
}
