using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NFSolidaria.Core.Dto
{
	public class UsuarioDtoSpecializedResult : UsuarioDto
	{

        public IEnumerable<CupomDto> CollectionCupom { get; set;} 
        public IEnumerable<UsuarioEntidadeFavoritaDto> CollectionUsuarioEntidadeFavorita { get; set;} 
        public  CadastradorDto Cadastrador { get; set;} 
        public  EntidadeDto Entidade { get; set;} 

		
	}
}