using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.Interfaces;
using Common.Infrastructure.Log;
using SimpleInjector;
using Common.Infrastructure.Cache;
using Common.Infrastructure.ORM.Context;
using Common.Infrastructure.ORM.Repositories;
using NFSolidaria.Core.Infrastructure.ORM.Contexto;
using Common.Cripto;


public static partial class ConfigContainer
{

    public static void RegisterOtherComponents(Container container)
    {
        container.Register<ICripto, Cripto>(Lifestyle.Scoped);

    }

}

