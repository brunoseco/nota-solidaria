using NFSolidaria.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NFSolidaria.Core.Infrastructure.ORM.Maps
{
    public partial class EntidadeCadastradorMap : EntityTypeConfiguration<EntidadeCadastrador>
    {
        public EntidadeCadastradorMap()
        {

            this.ToTable("EntidadeCadastrador");
            this.Property(t => t.EntidadeId).HasColumnName("EntidadeId");
            this.Property(t => t.CadastradorId).HasColumnName("CadastradorId");
           




            this.HasKey(d => new { d.EntidadeId,d.CadastradorId, }); 

			this.CustomConfig();

        }
		
    }
}