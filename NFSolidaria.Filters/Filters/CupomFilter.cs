using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSolidaria.Core.Filters
{
    public partial class CupomFilter : Filter
    {

        public int CupomId { get; set;} 
        public string ChaveAcesso { get; set;} 
        public DateTime DataCompraStart { get; set;} 
        public DateTime DataCompraEnd { get; set;} 
        public DateTime DataCompra { get; set;} 
        public string COO { get; set;} 
        public int TipoNota { get; set;} 
        public decimal Valor { get; set;} 
        public int EntidadeId { get; set;} 
        public int UsuarioId { get; set;} 
        public int? CadastradorId { get; set;} 
        public string CNPJEmissor { get; set;} 
        public int Situacao { get; set;} 
        public DateTime DataLancamentoStart { get; set;} 
        public DateTime DataLancamentoEnd { get; set;} 
        public DateTime DataLancamento { get; set;} 
        public DateTime? DataProcessamentoStart { get; set;} 
        public DateTime? DataProcessamentoEnd { get; set;} 
        public DateTime? DataProcessamento { get; set;} 
        public byte[] Imagem1 { get; set;} 
        public byte[] Imagem2 { get; set;} 


    }
}
