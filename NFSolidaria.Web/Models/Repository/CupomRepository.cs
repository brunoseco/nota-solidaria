using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRankingNFE.Models.Repository
{
    public class CupomRepository : RepositoryBase<Cupom>
    {
        public IEnumerable<Cupom> ObterDados()
        {
            var dados = base.GetAll().GroupBy(m => new { m.DataCompra.Month, m.Entidade, m.Usuario }).Select(m => new
            {
                entidadeid = m.Key.Entidade.Id,
                nome = m.Key.Usuario.Nome,
                mes = m.Key.Month,
                valor = m.Sum(p => p.Valor)
            }).ToList();

            return dados.Select(m => new Cupom
            {
                Valor = m.valor,
                Usuario = new Usuario
                {
                    Id = m.entidadeid,
                    Nome = m.nome                    
                }           
            });
        }
    }
}
