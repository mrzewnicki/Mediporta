namespace Mediporta.Data.Entities;

public class TagCollective
{
    public int TagId { get; set; }

    public virtual Tag Tag { get; set; }

    public int CollectiveId { get; set; }

    public virtual Collective Collective { get; set; }
}