using NFSolidaria.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NFSolidaria.Core.Infrastructure.ORM.Maps
{
    public partial class UsuarioMap : EntityTypeConfiguration<Usuario>
    {

		public void CustomConfig()
		{
			


		}

    }
}