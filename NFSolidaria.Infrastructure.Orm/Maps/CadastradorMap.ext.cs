using NFSolidaria.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NFSolidaria.Core.Infrastructure.ORM.Maps
{
    public partial class CadastradorMap : EntityTypeConfiguration<Cadastrador>
    {

		public void CustomConfig()
		{
            this.HasRequired(_ => _.Usuario).WithOptional(_ => _.Cadastrador);



        }

    }
}