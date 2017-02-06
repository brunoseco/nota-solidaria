using NFSolidaria.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NFSolidaria.Core.Infrastructure.ORM.Maps
{
    public partial class CadastradorMap : EntityTypeConfiguration<Cadastrador>
    {
        public CadastradorMap()
        {

            this.ToTable("Cadastrador");
            this.Property(t => t.CadastradorId).HasColumnName("Id");
           

            this.Property(t => t.Pass).HasColumnName("Pass");
            this.Property(t => t.Ativo).HasColumnName("Ativo");



            this.HasKey(d => new { d.CadastradorId, }); 

			this.CustomConfig();

        }
		
    }
}