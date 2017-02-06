using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace NFSolidaria.Core.Domain
{	
	public partial class CadastradorValidation
	{
        [MaxLength(100, ErrorMessage = "Cadastrador - Quantidade de caracteres maior que o permitido para o campo Pass")]
        public virtual object Pass {get; set;}

        [Required(ErrorMessage="Cadastrador - Campo Ativo é Obrigatório")]
        public virtual object Ativo {get; set;}


	}
}