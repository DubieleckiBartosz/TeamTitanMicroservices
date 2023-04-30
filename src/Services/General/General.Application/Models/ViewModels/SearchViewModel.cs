namespace General.Application.Models.ViewModels;

public class SearchViewModel<T>
{
    public List<T> Items { get; init; } = new();
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public bool HasPrevious { get; init; }
    public bool HasNext { get; init; } 
}