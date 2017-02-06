using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSolidaria.Core.Filters
{
    public partial class EntidadeFilter : Filter
    {

        public int EntidadeId { get; set;} 
        public string IdentificadorNFP { get; set;} 
        public bool? Ativo { get; set;}
    }
}
