using Shared.Implementations.Search.SearchParameters;

namespace Shared.Implementations.Search;

public interface IFilterModel
{
    SortModelParameters Sort { get; set; }
}