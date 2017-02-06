using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    [Serializable]
    public abstract class DtoBase
    {
        public string AttributeBehavior { get; set; }

        public string ConfirmationResponseBehavior { get; set; }

        public string QueryOptimizerBehavior { get; set; }
        
    }
}
