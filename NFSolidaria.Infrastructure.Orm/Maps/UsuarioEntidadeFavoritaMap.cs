using NFSolidaria.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NFSolidaria.Core.Infrastructure.ORM.Maps
{
    public partial class UsuarioEntidadeFavoritaMap : EntityTypeConfiguration<UsuarioEntidadeFavorita>
    {
        public UsuarioEntidadeFavoritaMap()
        {

            this.ToTable("UsuarioEntidadeFavorita");
            this.Property(t => t.UsuarioId).HasColumnName("UsuarioId");
            this.Property(t => t.EntidadeId).HasColumnName("EntidadeId");
           




            this.HasKey(d => new { d.UsuarioId,d.EntidadeId, }); 

			this.CustomConfig();

        }
		
    }
}