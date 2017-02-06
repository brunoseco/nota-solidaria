using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    public class UniqueListInfo : List<Info>
    {

        public new void Add(Info item)
        {
            var exists = this.Where(_ => _.Table == item.Table).Where(_ => _.PropertyName == item.PropertyName).Any();
            if (!exists)
                base.Add(item);

        }

        public new void AddRange(IEnumerable<Info> items)
        {
            foreach (var item in items)
                this.Add(item);

        }




    }
}
