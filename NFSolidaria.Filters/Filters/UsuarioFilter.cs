using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSolidaria.Core.Filters
{
    public partial class UsuarioFilter : Filter
    {

        public int UsuarioId { get; set;} 
        public string CPF_CNPJ { get; set;} 
        public string Nome { get; set;} 
        public string RazaoSocial { get; set;} 
        public string Email { get; set;} 
        public DateTime? DataNascimentoStart { get; set;} 
        public DateTime? DataNascimentoEnd { get; set;} 
        public DateTime? DataNascimento { get; set;} 
        public string SenhaMD5 { get; set;} 
        public string Cidade { get; set;} 
        public string UF { get; set;} 
        public string LastToken { get; set;} 


    }
}
