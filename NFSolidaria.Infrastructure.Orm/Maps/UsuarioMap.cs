using NFSolidaria.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NFSolidaria.Core.Infrastructure.ORM.Maps
{
    public partial class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {

            this.ToTable("Usuario");
            this.Property(t => t.UsuarioId).HasColumnName("Id");
           

            this.Property(t => t.CPF_CNPJ).HasColumnName("CPF_CNPJ");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.RazaoSocial).HasColumnName("RazaoSocial");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.DataNascimento).HasColumnName("DataNascimento");
            this.Property(t => t.SenhaMD5).HasColumnName("SenhaMD5");
            this.Property(t => t.Cidade).HasColumnName("Cidade");
            this.Property(t => t.UF).HasColumnName("UF");
            this.Property(t => t.LastToken).HasColumnName("LastToken");



            this.HasKey(d => new { d.UsuarioId, }); 

			this.CustomConfig();

        }
		
    }
}