using System.ComponentModel.DataAnnotations;

namespace Mediporta.Data.Entities;

public class Collective
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public string? Link { get; set; }

    // 1 Collective to Many Tags
    public ICollection<TagCollective> TagCollectives { get; set; }

    public ICollection<ExternalLink> ExternalLinks { get; set; }
}