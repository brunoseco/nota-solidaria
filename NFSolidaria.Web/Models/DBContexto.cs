using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRankingNFE.Models
{
    public class DBContexto : DbContext
    {
        public DBContexto() : base("DBCon")
        {
            Database.SetInitializer<DBContexto>(new DBContextInicializer());
        }



        public virtual DbSet<Cadastrador> Cadastrador { get; set; }
        public virtual DbSet<Cupom> Cupom { get; set; }
        public virtual DbSet<Entidade> Entidade { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cadastrador>()
                .HasMany(e => e.Entidade)
                .WithMany(e => e.Cadastrador)
                .Map(m => m.ToTable("EntidadeCadastrador").MapLeftKey("CadastradorId").MapRightKey("EntidadeId"));

            modelBuilder.Entity<Cupom>()
                .Property(e => e.ChaveAcesso)
                .IsUnicode(false);

            modelBuilder.Entity<Cupom>()
                .Property(e => e.COO)
                .IsUnicode(false);

            modelBuilder.Entity<Cupom>()
                .Property(e => e.CNPJEmissor)
                .IsUnicode(false);

            modelBuilder.Entity<Entidade>()
                .Property(e => e.IdentificadorNFP)
                .IsUnicode(false);

            modelBuilder.Entity<Entidade>()
                .HasMany(e => e.Cupom)
                .WithRequired(e => e.Entidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Entidade>()
                .HasMany(e => e.Usuario1)
                .WithMany(e => e.Entidade1)
                .Map(m => m.ToTable("UsuarioEntidadeFavorita").MapLeftKey("EntidadeId").MapRightKey("UsuarioId"));

            modelBuilder.Entity<Usuario>()
                .Property(e => e.CPF_CNPJ)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.RazaoSocial)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.SenhaMD5)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Cidade)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.UF)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.LastToken)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .HasOptional(e => e.Cadastrador)
                .WithRequired(e => e.Usuario);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Cupom)
                .WithRequired(e => e.Usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasOptional(e => e.Entidade)
                .WithRequired(e => e.Usuario);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

        }
    }
}
