using Common.Domain.Interfaces;
using Common.Infrastructure.Cache;
using Common.ServiceLocator;

namespace NFSolidaria.Integration.Application.Config
{
    public class ServiceLocatorConfigIntegration : ServiceLocator
    {
        public override void BuildServiceTypesMap()
        {
            base.ServiceTypeAdd(typeof(ICache), typeof(FactoryCache));

        }
    }
}
