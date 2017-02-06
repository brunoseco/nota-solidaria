using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace NFSolidaria.Core.Domain
{	
	public partial class CupomValidation
	{
        [MaxLength(100, ErrorMessage = "Cupom - Quantidade de caracteres maior que o permitido para o campo ChaveAcesso")]
        public virtual object ChaveAcesso {get; set;}

        [Required(ErrorMessage="Cupom - Campo DataCompra é Obrigatório")]
        public virtual object DataCompra {get; set;}

        [Required(ErrorMessage="Cupom - Campo COO é Obrigatório")]
        [MaxLength(20, ErrorMessage = "Cupom - Quantidade de caracteres maior que o permitido para o campo COO")]
        public virtual object COO {get; set;}

        [Required(ErrorMessage="Cupom - Campo TipoNota é Obrigatório")]
        public virtual object TipoNota {get; set;}

        [Required(ErrorMessage="Cupom - Campo Valor é Obrigatório")]
        public virtual object Valor {get; set;}

        [Required(ErrorMessage="Cupom - Campo CNPJEmissor é Obrigatório")]
        [MaxLength(25, ErrorMessage = "Cupom - Quantidade de caracteres maior que o permitido para o campo CNPJEmissor")]
        public virtual object CNPJEmissor {get; set;}

        [Required(ErrorMessage="Cupom - Campo Situacao é Obrigatório")]
        public virtual object Situacao {get; set;}

        [Required(ErrorMessage="Cupom - Campo DataLancamento é Obrigatório")]
        public virtual object DataLancamento {get; set;}


	}
}