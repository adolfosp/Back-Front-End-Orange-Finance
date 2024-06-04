namespace OrangeFinance.Domain.Common.Models;

public struct Pagination
{
    public int Page { get; }
    public int PageSize { get; }

    public Pagination(int page, int pageSize)
    {
        if (page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(page), "Page number cannot be less than 1.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size cannot be less than 1.");
        }

        Page = page;
        PageSize = pageSize;
    }
}
