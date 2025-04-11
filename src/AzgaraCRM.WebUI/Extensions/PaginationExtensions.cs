using AzgaraCRM.WebUI.Models;

namespace AzgaraCRM.WebUI.Extensions;

public static class PaginationExtensions
{
    public static PaginationResult<T> ToPaginate<T>(this IQueryable<T> source, PaginationParameters? pagination)
    {
        if (pagination is null)
            return new PaginationResult<T> { Data = source, PaginationInfo = null };

        int totalCount = source.Count();

        source = pagination.PageNumber > 0 && pagination.PageSize > 0
            ? source.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize)
            : source;

        return new PaginationResult<T> { Data = source, PaginationInfo = new PaginationInfo(totalCount, pagination), };
    }
}