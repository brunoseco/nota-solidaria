using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Dto;

namespace NFSolidaria.Core.Dto
{
	public class EntidadeDto  : DtoBase
	{
	
        public int EntidadeId { get; set;} 
        public string IdentificadorNFP { get; set;} 
        public bool Ativo { get; set;} 

		
	}
}