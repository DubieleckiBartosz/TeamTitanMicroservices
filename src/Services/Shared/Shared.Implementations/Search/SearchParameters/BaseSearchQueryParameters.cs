namespace Shared.Implementations.Search.SearchParameters;

public abstract class BaseSearchQueryParameters
{
    private const int MaxPageSize = 100;
    private const int DefaultPageNumber = 1;
    private int _pageNumber = 1;
    private int _pageSize = 10;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value is <= 0 ? DefaultPageNumber : value;
    }
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value is > MaxPageSize ? MaxPageSize : value is <= 0 ? _pageSize : value;
    }
}