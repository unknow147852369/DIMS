using DIMSApis.Models.Helper;

namespace DIMSApis.Interfaces
{
    public interface IPaginationService
    {
        Task<Pagination<T>> GetPagination<T>(IQueryable<T> query, int page,  int pageSize) where T : class;
    }
}
