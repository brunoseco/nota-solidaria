using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Common.Models
{
    [Serializable]
    public class Summary
    {

        public int Total { get; set; }

        public object AdditionalSummary { get; set; }
      

    }


}
