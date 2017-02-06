using Common.Domain.Interfaces;
using Common.Infrastructure.Cache;
using Common.ServiceLocator;

namespace NFSolidaria.Application.Config
{
    public class ServiceLocatorConfigAdministative : ServiceLocator
    {
        public override void BuildServiceTypesMap()
        {
            base.ServiceTypeAdd(typeof(ICache), typeof(FactoryCache));

        }
    }
}
