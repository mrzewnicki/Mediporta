using System.ComponentModel.DataAnnotations;

namespace Mediporta.Data.Entities;

public class Tag
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public int Count { get; set; }

    public bool HasSynonyms { get; set; }

    public bool IsModeratorOnly { get; set; }

    public bool IsRequired { get; set; }

    // Many Tags to Many Collectives
    public ICollection<TagCollective> TagCollectives { get; set; }
}