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
using SimpleInjector.Integration.Web;

public static partial class ConfigContainer
{

    private static Container container = new Container();

    static ConfigContainer()
    {
		container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
		container.Register<ILog, LogFileComponent>(Lifestyle.Scoped);
		container.Register<IUnitOfWork<DbContextCore>, DbContextCore>(Lifestyle.Scoped);
		container.Register<ICache, FactoryCache>(Lifestyle.Singleton);

        container.Register<IRepository<NFSolidaria.Core.Domain.Cadastrador>, Repository<NFSolidaria.Core.Domain.Cadastrador, DbContextCore>>(Lifestyle.Scoped);
        container.Register<IRepository<NFSolidaria.Core.Domain.Cupom>, Repository<NFSolidaria.Core.Domain.Cupom, DbContextCore>>(Lifestyle.Scoped);
        container.Register<IRepository<NFSolidaria.Core.Domain.Entidade>, Repository<NFSolidaria.Core.Domain.Entidade, DbContextCore>>(Lifestyle.Scoped);
        container.Register<IRepository<NFSolidaria.Core.Domain.EntidadeCadastrador>, Repository<NFSolidaria.Core.Domain.EntidadeCadastrador, DbContextCore>>(Lifestyle.Scoped);
        container.Register<IRepository<NFSolidaria.Core.Domain.Usuario>, Repository<NFSolidaria.Core.Domain.Usuario, DbContextCore>>(Lifestyle.Scoped);
        container.Register<IRepository<NFSolidaria.Core.Domain.UsuarioEntidadeFavorita>, Repository<NFSolidaria.Core.Domain.UsuarioEntidadeFavorita, DbContextCore>>(Lifestyle.Scoped);


		RegisterOtherComponents(container);

        container.Verify();

    }

    public static Container Container()
    {
        return container;
    }

}

