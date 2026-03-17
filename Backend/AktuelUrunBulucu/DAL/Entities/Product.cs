using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AktuelUrunBulucu.DAL.Entities;

[Table("products")]
public class Product
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [Required]
    public string Name { get; set; } = string.Empty;

    [Column("category")]
    [Required]
    public string Category { get; set; } = string.Empty;

    [Column("store_name")]
    [Required]
    public string StoreName { get; set; } = string.Empty;

    [Column("product_bring_date")]
    public DateTime? ProductBringDate { get; set; }
}
