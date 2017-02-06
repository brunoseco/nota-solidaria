namespace ProjetoRankingNFE.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Usuario")]
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            Cupom = new HashSet<Cupom>();
            Entidade1 = new HashSet<Entidade>();
        }

        public int Id { get; set; }

        [StringLength(14)]
        public string CPF_CNPJ { get; set; }

        [Required]
        [StringLength(150)]
        public string Nome { get; set; }

        [Required]
        [StringLength(150)]
        public string RazaoSocial { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        public DateTime? DataNascimento { get; set; }

        [Required]
        [StringLength(32)]
        public string SenhaMD5 { get; set; }

        [StringLength(100)]
        public string Cidade { get; set; }

        [StringLength(2)]
        public string UF { get; set; }

        [StringLength(200)]
        public string LastToken { get; set; }

        public virtual Cadastrador Cadastrador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cupom> Cupom { get; set; }

        public virtual Entidade Entidade { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Entidade> Entidade1 { get; set; }
    }
}
