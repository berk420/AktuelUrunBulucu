using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AktuelUrunBulucu.DAL.Entities;

[Table("search_logs")]
public class SearchLog
{
    [Column("id")]
    public int Id { get; set; }

    [Column("ip_address")]
    [Required]
    public string IpAddress { get; set; } = string.Empty;

    [Column("searched_product")]
    [Required]
    public string SearchedProduct { get; set; } = string.Empty;

    [Column("searched_at")]
    public DateTime SearchedAt { get; set; }
}
