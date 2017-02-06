using Common.Domain.CustomExceptions;
using Common.Domain.Helper;
using Common.Domain.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public abstract class DomainBase : IDisposable
    {

        protected string token;

        protected string toolName;

        [NotMapped]
        public int TotalCount { get; set; }

        [NotMapped]
        public string AttributeBehavior { get; set; }

        [NotMapped]
        public string ConfirmationResponseBehavior { get; set; }

        [NotMapped]
        public string QueryOptimizerBehavior { get; set; }

        [NotMapped]
        public string SummaryBehavior { get; set; }

        [NotMapped]
        public object Parameters { get; set; }

        public virtual void SetToken(string token)
        {
            this.token = token;
        }

        public virtual void Init()
        {
        }

        protected PaginateResult<T> Paging<T>(Filter filter, IQueryable<T> querySorted) where T : class
        {
            return Paging(filter, querySorted, querySorted);
        }
        protected PaginateResult<T> Paging<T>(Filter filter, IQueryable<T> query, IQueryable<T> querySorted) where T : class
        {
            var totalCount = query.Count();
            if (totalCount <= filter.PageSize)
            {
                return new PaginateResult<T>
                {
                    ResultPaginatedData = querySorted,
                    TotalCount = totalCount,
                    Source = query
                };
            }

            if (filter.IsPagination)
                return querySorted.PaginateNew(querySorted, filter.PageIndex, filter.PageSize, totalCount);

            return new PaginateResult<T>
            {
                ResultPaginatedData = querySorted,
                TotalCount = totalCount,
                Source = query
            };
        }

        public abstract void Dispose();
    }
}
