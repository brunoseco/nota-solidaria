using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NFSolidaria.Core.Dto
{
	public class CadastradorDtoSpecializedResult : CadastradorDto
	{

        public IEnumerable<CupomDto> CollectionCupom { get; set;} 
        public IEnumerable<EntidadeCadastradorDto> CollectionEntidadeCadastrador { get; set;} 
        public  UsuarioDto Usuario { get; set;} 

		
	}
}