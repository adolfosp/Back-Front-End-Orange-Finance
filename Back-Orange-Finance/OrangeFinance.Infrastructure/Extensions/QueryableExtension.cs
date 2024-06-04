using OrangeFinance.Domain.Common.Models;

namespace OrangeFinance.Infrastructure.Extensions;

internal static class QueryableExtension
{
    public static IQueryable<T> Paginate<T>(
      this IQueryable<T> source,
      Pagination pagination)
    {
        return source
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize);
    }
}
