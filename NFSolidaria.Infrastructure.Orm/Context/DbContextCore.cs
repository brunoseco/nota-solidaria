using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Infrastructure.ORM.Context;
using Common.Infrastructure.Log;
using NFSolidaria.Core.Infrastructure.ORM.Maps;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure.MappingViews;
using NFSolidaria.Core.Infrastructure.ORM.Contexto;
using Common.Domain.Interfaces;


namespace NFSolidaria.Core.Infrastructure.ORM.Contexto
{
    public class DbContextCore : DbContext, IUnitOfWork<DbContextCore>,IUnitOfWork
    {
        static DbContextCore()
        {
            Database.SetInitializer<DbContextCore>(null);
        }

        public DbContextCore(ILog log)
            : base(ConfigurationManager.ConnectionStrings["Core"].ConnectionString)
        {
			base.Database.Log = log.Info;
        }
		
		public string ConnectionStringComplete()
        {
            return ConfigurationManager.ConnectionStrings["Core"].ConnectionString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new CadastradorMap());
            modelBuilder.Configurations.Add(new CupomMap());
            modelBuilder.Configurations.Add(new EntidadeMap());
            modelBuilder.Configurations.Add(new EntidadeCadastradorMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
            modelBuilder.Configurations.Add(new UsuarioEntidadeFavoritaMap());


        }


        public void Commit()
        {
            base.SaveChanges();
        }
    }
}
