using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Dto;

namespace NFSolidaria.Core.Dto
{
	public class CadastradorDto  : DtoBase
	{
	
        public int CadastradorId { get; set;} 
        public string Pass { get; set;} 
        public bool Ativo { get; set;} 

		
	}
}