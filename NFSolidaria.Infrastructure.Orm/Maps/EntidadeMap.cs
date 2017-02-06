using NFSolidaria.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NFSolidaria.Core.Infrastructure.ORM.Maps
{
    public partial class EntidadeMap : EntityTypeConfiguration<Entidade>
    {
        public EntidadeMap()
        {

            this.ToTable("Entidade");
            this.Property(t => t.EntidadeId).HasColumnName("Id");
           

            this.Property(t => t.IdentificadorNFP).HasColumnName("IdentificadorNFP");
            this.Property(t => t.Ativo).HasColumnName("Ativo");



            this.HasKey(d => new { d.EntidadeId, }); 

			this.CustomConfig();

        }
		
    }
}