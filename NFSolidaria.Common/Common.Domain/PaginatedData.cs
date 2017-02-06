using System;
using System.Collections.Generic;
using System.Linq;

public class PaginateResult<T> {

    public IEnumerable<T> ResultPaginatedData { get; set; }

    public IEnumerable<dynamic> ResultPaginatedDataDynamic { get; set; }

    public IQueryable<T> Source { get; set; }

    public int TotalCount { get; set; }

}
public class PaginatedData<T> : List<T> where T : class
{
    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; }

    public PaginatedData(IQueryable<T> source, int pageIndex, int pageSize, int totalCount)
    {
        this.Paging(source, pageIndex, pageSize, totalCount);
    }
    public PaginatedData(IQueryable<T> source, int pageIndex, int pageSize)
    {
        this.Paging(source, pageIndex, pageSize, source.Count());
    }

    public bool HasPreviousPage
    {
        get
        {
            return (PageIndex > 0);
        }
    }

    public bool HasNextPage
    {
        get
        {
            return (PageIndex + 1 < TotalPages);
        }
    }

    private void Paging(IQueryable<T> source, int pageIndex, int pageSize, int totalCount)
    {
        this.PageIndex = pageIndex.IsMoreThanZero() ? pageIndex - 1 : 0;
        this.PageSize = pageSize;
        this.TotalCount = totalCount;

        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
        var result = source.Skip(PageIndex * PageSize).Take(PageSize);
        this.AddRange(result);
    }
}

