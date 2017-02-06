using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSolidaria.Core.Domain.Custom
{
    [Serializable]
    public class UsuarioCache
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string RazaoSocial { get; set; }
        public string Email { get; set; }
        public string LastToken { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
    }
}
