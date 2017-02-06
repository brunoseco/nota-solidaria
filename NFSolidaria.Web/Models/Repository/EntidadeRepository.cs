using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRankingNFE.Models.Repository
{
    public class EntidadeRepository : RepositoryBase<Entidade>
    {
        public IEnumerable<dynamic> ObterDadosPorMes(int mes)
        {
            return base.GetAll().Select(m => new
            {
                name = m.Usuario.Nome,
                data = m.Cupom
                .Where(p => p.DataCompra.Month * 100 == mes * 100)
                .Where(p => p.Situacao == 2)
                .GroupBy(n => new { n.DataCompra.Month })
                .Select(n => n.Sum(p => p.Valor)).ToList()
            }).ToList();
        }

        public IEnumerable<dynamic> ObterDadosPorAno()
        {
            return base.GetAll().OrderByDescending(m => m.ValorTotal);
        }
    }
}
