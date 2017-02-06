using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSolidaria.Core.Filters
{
    public partial class UsuarioEntidadeFavoritaFilter : Filter
    {

        public int UsuarioId { get; set;} 
        public int EntidadeId { get; set;} 


    }
}
