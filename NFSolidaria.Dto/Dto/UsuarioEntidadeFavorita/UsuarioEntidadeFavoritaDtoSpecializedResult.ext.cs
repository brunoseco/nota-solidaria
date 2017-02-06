using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NFSolidaria.Core.Dto
{
	public class UsuarioEntidadeFavoritaDtoSpecializedResult : UsuarioEntidadeFavoritaDto
	{

        public  EntidadeDtoSpecializedResult Entidade { get; set;} 
        public  UsuarioDto Usuario { get; set;} 

		
	}
}