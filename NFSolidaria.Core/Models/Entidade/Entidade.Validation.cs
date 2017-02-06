using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace NFSolidaria.Core.Domain
{	
	public partial class EntidadeValidation
	{
        [MaxLength(100, ErrorMessage = "Entidade - Quantidade de caracteres maior que o permitido para o campo IdentificadorNFP")]
        public virtual object IdentificadorNFP {get; set;}

        [Required(ErrorMessage="Entidade - Campo Ativo é Obrigatório")]
        public virtual object Ativo {get; set;}


	}
}