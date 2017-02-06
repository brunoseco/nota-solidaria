using AutoMapper;

namespace NFSolidaria.Integration.Application.Config
{
	public class AutoMapperConfigCore
    {
		public static void RegisterMappings()
		{

			Mapper.Initialize(x =>
			{
				x.AddProfile<DominioToDtoProfileCore>();
			});

		}
	}
}
