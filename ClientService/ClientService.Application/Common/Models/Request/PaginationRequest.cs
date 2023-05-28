using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using ClientService.Application.Common.Constants;
using ClientService.Application.Common.Enums;

namespace ClientService.Application.Common.Models.Request;

public abstract class PaginationRequest<T> where T : class
{
    private int _pageNumber = DefaultPagination.DefaultPageNumber;

    private int _pageSize = DefaultPagination.DefaultPageSize;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value > 0
            ? value
            : DefaultPagination.DefaultPageNumber;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 0 && value <= DefaultPagination.MaxPageSize
            ? value
            : DefaultPagination.DefaultPageSize;
    }

    public string? SortColumn { get; set; }

    public SortDirection SortDir { get; set; } = SortDirection.Asc;

    public abstract Expression<Func<T, bool>> GetExpressions();

    public Func<IQueryable<T>, IOrderedQueryable<T>>? GetOrder()
    {
        if (string.IsNullOrWhiteSpace(SortColumn)) return null;

        return query => query.OrderBy($"{SortColumn} {SortDir.ToString().ToLower()}");
    }
}
