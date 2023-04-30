﻿namespace General.Application.Models.Wrappers;

public class ListWrapper<T>
{
    public List<T> Items { get; private set; } = new();
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    private ListWrapper(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items.AddRange(items);
    }

    public static ListWrapper<T> Wrap(List<T> items, int count, int pageNumber, int pageSize)
    { 
        return new ListWrapper<T>(items, count, pageNumber, pageSize);
    }
}