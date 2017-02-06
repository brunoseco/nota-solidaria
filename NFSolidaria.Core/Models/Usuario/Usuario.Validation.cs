using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace NFSolidaria.Core.Domain
{	
	public partial class UsuarioValidation
	{
        [MaxLength(14, ErrorMessage = "Usuario - Quantidade de caracteres maior que o permitido para o campo CPF_CNPJ")]
        public virtual object CPF_CNPJ {get; set;}

        [Required(ErrorMessage="Usuario - Campo Nome é Obrigatório")]
        [MaxLength(150, ErrorMessage = "Usuario - Quantidade de caracteres maior que o permitido para o campo Nome")]
        public virtual object Nome {get; set;}

        [Required(ErrorMessage="Usuario - Campo RazaoSocial é Obrigatório")]
        [MaxLength(150, ErrorMessage = "Usuario - Quantidade de caracteres maior que o permitido para o campo RazaoSocial")]
        public virtual object RazaoSocial {get; set;}

        [Required(ErrorMessage="Usuario - Campo Email é Obrigatório")]
        [MaxLength(100, ErrorMessage = "Usuario - Quantidade de caracteres maior que o permitido para o campo Email")]
        public virtual object Email {get; set;}

        [Required(ErrorMessage="Usuario - Campo SenhaMD5 é Obrigatório")]
        [MaxLength(32, ErrorMessage = "Usuario - Quantidade de caracteres maior que o permitido para o campo SenhaMD5")]
        public virtual object SenhaMD5 {get; set;}

        [MaxLength(100, ErrorMessage = "Usuario - Quantidade de caracteres maior que o permitido para o campo Cidade")]
        public virtual object Cidade {get; set;}

        [MaxLength(2, ErrorMessage = "Usuario - Quantidade de caracteres maior que o permitido para o campo UF")]
        public virtual object UF {get; set;}

        [MaxLength(200, ErrorMessage = "Usuario - Quantidade de caracteres maior que o permitido para o campo LastToken")]
        public virtual object LastToken {get; set;}


	}
}