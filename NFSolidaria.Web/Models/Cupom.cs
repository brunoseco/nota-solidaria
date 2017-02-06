namespace ProjetoRankingNFE.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cupom")]
    public partial class Cupom
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string ChaveAcesso { get; set; }

        public DateTime DataCompra { get; set; }

        [Required]
        [StringLength(20)]
        public string COO { get; set; }

        public int TipoNota { get; set; }

        public decimal Valor { get; set; }

        public int EntidadeId { get; set; }

        public int UsuarioId { get; set; }

        public int? CadastradorId { get; set; }

        [Required]
        [StringLength(25)]
        public string CNPJEmissor { get; set; }

        public int Situacao { get; set; }

        public DateTime DataLancamento { get; set; }

        public DateTime? DataProcessamento { get; set; }

        [Column(TypeName = "image")]
        public byte[] Imagem1 { get; set; }

        [Column(TypeName = "image")]
        public byte[] Imagem2 { get; set; }

        public virtual Cadastrador Cadastrador { get; set; }

        public virtual Entidade Entidade { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
