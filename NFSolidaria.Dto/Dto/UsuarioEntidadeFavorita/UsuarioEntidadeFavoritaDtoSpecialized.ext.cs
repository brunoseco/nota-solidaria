using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NFSolidaria.Core.Dto
{
	public class UsuarioEntidadeFavoritaDtoSpecialized : UsuarioEntidadeFavoritaDto
	{

        public  EntidadeDto Entidade { get; set;} 
        public  UsuarioDto Usuario { get; set;} 

		
	}
}