using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Dto;

namespace NFSolidaria.Core.Dto
{
	public class UsuarioDto  : DtoBase
	{
	
        public int UsuarioId { get; set;} 
        public string CPF_CNPJ { get; set;} 
        public string Nome { get; set;} 
        public string RazaoSocial { get; set;} 
        public string Email { get; set;} 
        public DateTime? DataNascimento { get; set;} 
        public string SenhaMD5 { get; set;} 
        public string Cidade { get; set;} 
        public string UF { get; set;} 
        public string LastToken { get; set;} 

		
	}
}