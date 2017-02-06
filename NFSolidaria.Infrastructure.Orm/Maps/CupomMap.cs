using NFSolidaria.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NFSolidaria.Core.Infrastructure.ORM.Maps
{
    public partial class CupomMap : EntityTypeConfiguration<Cupom>
    {
        public CupomMap()
        {

            this.ToTable("Cupom");
            this.Property(t => t.CupomId).HasColumnName("Id");
           

            this.Property(t => t.ChaveAcesso).HasColumnName("ChaveAcesso");
            this.Property(t => t.DataCompra).HasColumnName("DataCompra");
            this.Property(t => t.COO).HasColumnName("COO");
            this.Property(t => t.TipoNota).HasColumnName("TipoNota");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.EntidadeId).HasColumnName("EntidadeId");
            this.Property(t => t.UsuarioId).HasColumnName("UsuarioId");
            this.Property(t => t.CadastradorId).HasColumnName("CadastradorId");
            this.Property(t => t.CNPJEmissor).HasColumnName("CNPJEmissor");
            this.Property(t => t.Situacao).HasColumnName("Situacao");
            this.Property(t => t.DataLancamento).HasColumnName("DataLancamento");
            this.Property(t => t.DataProcessamento).HasColumnName("DataProcessamento");
            this.Property(t => t.Imagem1).HasColumnName("Imagem1");
            this.Property(t => t.Imagem2).HasColumnName("Imagem2");



            this.HasKey(d => new { d.CupomId, }); 

			this.CustomConfig();

        }
		
    }
}