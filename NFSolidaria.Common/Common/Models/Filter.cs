using Common.Enum;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public abstract class Filter : IFilter
    {

        public Filter()
        {
            this.IsPagination = true;
            this.PageIndex = 0;
            this.PageSize = 50;
            this.ByCurrentUser = true;
            this.CacheExpiresTime = new TimeSpan(24, 0, 0);
        }

        public TimeSpan CacheExpiresTime { get; set; }
        public int CacheGroupId { get; set; }
        public string CacheGroup { get; set; }
        public bool ByCache { get; set; }
        public bool ByCurrentUser { get; set; }
        public string AttributeBehavior { get; set; }
        public string SummaryBehavior { get; set; }
        public string QueryOptimizerBehavior { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string FilterName { get; set; }
        public string[] OrderFields { get; set; }
        public OrderByType orderByType { get; set; }
        public bool IsOnlySummary { get; set; }
        public bool IsPagination { get; set; }
        public bool IsOrderByDomain { get; set; }
        public bool IsOrderByDynamic { get; set; }

        public void OrderBy(params string[] OrderFields)
        {
            this.OrderFields = OrderFields;
        }
    }
}
