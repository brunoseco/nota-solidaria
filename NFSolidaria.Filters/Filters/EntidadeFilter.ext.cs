using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSolidaria.Core.Filters
{
    public partial class EntidadeFilter 
    {
        public int NaoFavoritadaPeloUsuarioId { get; set; }
        public bool ComUsuarioCadastrador { get; set; }
        public bool ComCupomPendente { get; set; }
        public string Nome { get; set; }

    }
}
