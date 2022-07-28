using DIMSApis.Interfaces;
using DIMSApis.Models.Helper;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Services
{
    public class PaginationService : IPaginationService
    {
        public async Task<Pagination<T>> GetPagination<T>(IQueryable<T> query, int page, int pageSize) where T : class
        {
            Pagination<T> pagination = new Pagination<T>();

            pagination.TotalItems = query.Count();
            pagination.PageSize = pageSize; 
            pagination.TotalPages = (pagination.TotalItems % pagination.PageSize == 0) ?
                                     (pagination.TotalItems / pagination.PageSize) : (pagination.TotalItems / pagination.PageSize + 1);

            pagination.CurrentPage = page > 0 && page <= pagination.TotalPages ? page : 1;

            int skip = (pagination.CurrentPage - 1) * pageSize;


            pagination.Result = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return pagination;
        }
    }
}
