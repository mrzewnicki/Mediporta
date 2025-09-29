using Mediporta.Data.Entities;

namespace Mediporta.Data.Repositories.Helpers;

public record OrderByQuery(Func<Tag, object> KeySelector, bool IsAscending = true);
