using Common.Domain;
using Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class PaginateExtensions
{
    public static PaginateResult<T> PaginateNew<T>(this IQueryable<T> querySorted, IQueryable<T> query, int pageIndex = 0, int pageSize = 0, int totalCount = 0) where T : class
    {
        var paginationResult = new PaginatedData<T>(querySorted, pageIndex, pageSize, totalCount);

        return new PaginateResult<T>
        {
            ResultPaginatedData = paginationResult,
            TotalCount = paginationResult.TotalCount,
            Source = query
        };
    }
    public static PaginateResult<T> PaginateNew<T>(this IQueryable<T> source, int pageIndex = 0, int pageSize = 0) where T : class
    {
        var paginationResult = new PaginatedData<T>(source, pageIndex, pageSize);

        return new PaginateResult<T>
        {
            ResultPaginatedData = paginationResult,
            TotalCount = paginationResult.TotalCount,
            Source = source
        };
    }

    public static IEnumerable<T> Paginate<T>(this IQueryable<T> source, int pageIndex = 0, int pageSize = 10) where T : class
    {
        var paginationResult = new PaginatedData<T>(source, pageIndex, pageSize);
        return paginationResult;
    }
    
}








