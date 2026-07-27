namespace PCMS_Backend.Extensions;

public  static class PaginateExtension
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        //ensuring that paginate is not called if query paras are null
        if(query is null) throw new ArgumentNullException(nameof(query));

        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}
