using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NFSolidaria.Core.Dto
{
	public class CupomDtoSpecialized : CupomDto
	{

        public  CadastradorDto Cadastrador { get; set;} 
        public  EntidadeDtoSpecialized Entidade { get; set;} 
        public  UsuarioDto Usuario { get; set;} 

		
	}
}