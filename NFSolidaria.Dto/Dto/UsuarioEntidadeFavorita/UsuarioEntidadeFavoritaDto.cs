using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Dto;

namespace NFSolidaria.Core.Dto
{
	public class UsuarioEntidadeFavoritaDto  : DtoBase
	{
	
        public int UsuarioId { get; set;} 
        public int EntidadeId { get; set;} 

		
	}
}