using Microsoft.EntityFrameworkCore.Query;
using RestaurantListingAPI.Data.Helpers;
using RestaurantListingAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RestaurantListingAPI.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAllPaginated(
            PagingParams pagingParams,
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            );

        Task<PagedList<T>> GetAllPaginatedImproved(
            PagingParams pagingParams,
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            );

        Task<IList<T>> GetAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        Task<T> Get(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task Insert(T entity);
        void Update(T entity);
        Task InsertRange(IEnumerable<T> entities);
        void DeleteRange(IEnumerable<T> entities);
        Task Delete(int id);
    }
}
