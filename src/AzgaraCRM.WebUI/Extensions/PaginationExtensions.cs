using AzgaraCRM.WebUI.Domain.Models;
using AzgaraCRM.WebUI.Helpers;
using Newtonsoft.Json;

namespace AzgaraCRM.WebUI.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> ToPaginate<T>(this IQueryable<T> source, PaginationParameters? @params)
    {
        if (@params is null)
            return source;

        var totalCount = source.Count();
        var json = JsonConvert.SerializeObject(new PaginationInfo(totalCount, @params));

        HttpContextHelper.ResponseHeaders.Remove("X-Pagination");
        HttpContextHelper.ResponseHeaders?.Add("X-Pagination", json);

        source = @params.PageNumber > 0 && @params.PageSize > 0
            ? source.Skip((@params.PageNumber - 1) * @params.PageSize).Take(@params.PageSize)
            : source;

        return source;
    }
}