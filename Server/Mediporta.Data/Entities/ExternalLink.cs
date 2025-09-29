using System.ComponentModel.DataAnnotations;

namespace Mediporta.Data.Entities;

public class ExternalLink
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Type { get; set; }

    [Required]
    public string Link { get; set; }

    public int CollectiveId { get; set; }
    public virtual Collective Collective { get; set; }
}