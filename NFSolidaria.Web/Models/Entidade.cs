namespace ProjetoRankingNFE.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    [Table("Entidade")]
    public partial class Entidade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Entidade()
        {
            Cupom = new HashSet<Cupom>();
            Cadastrador = new HashSet<Cadastrador>();
            Usuario1 = new HashSet<Usuario>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(100)]
        public string IdentificadorNFP { get; set; }

        public bool Ativo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cupom> Cupom { get; set; }

        public virtual Usuario Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cadastrador> Cadastrador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Usuario> Usuario1 { get; set; }


        private IEnumerable<Cupom> CuponsProcessados()
        {
            return this.Cupom.Where(m => m.Situacao == 2);
        }

        [NotMapped]
        public int QuantidadeNotas
        {
            get
            {
                return this.CuponsProcessados().Count();
            }

        }

        [NotMapped]
        public decimal ValorTotal
        {
            get
            {
                return this.CuponsProcessados().Sum(m => m.Valor);
            }
        }
    }
}
