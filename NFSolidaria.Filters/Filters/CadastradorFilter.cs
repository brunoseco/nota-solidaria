using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSolidaria.Core.Filters
{
    public partial class CadastradorFilter : Filter
    {

        public int CadastradorId { get; set;} 
        public string Pass { get; set;} 
        public bool? Ativo { get; set;} 


    }
}
