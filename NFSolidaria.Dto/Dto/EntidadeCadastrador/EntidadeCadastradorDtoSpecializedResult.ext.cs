using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NFSolidaria.Core.Dto
{
	public class EntidadeCadastradorDtoSpecializedResult : EntidadeCadastradorDto
	{

        public  CadastradorDto Cadastrador { get; set;} 
        public  EntidadeDto Entidade { get; set;} 

		
	}
}