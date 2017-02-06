using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class DataItemFilter : IFilter
    {

        public int CacheGroupId { get; set; }

        public string DataItemType { get; set; }

        public int FilterId { get; set; }

        public int SubFilterId { get; set; }

        public string FilterValue { get; set; }

        public string SubFilterValue { get; set; }

        public bool ByCache { get; set; }
    }
}
