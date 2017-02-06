using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRankingNFE.Models
{
    public class EmailSendViewModel
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Mensagem { get; set; }

        [Required,EmailAddress]
        public string Email { get; set; }
    }
}
