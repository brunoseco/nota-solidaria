using Common.Gen;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSolidaria.Gen
{
    public class ConfigContext
    {

        private Context ConfigContextEtl()
        {
            return new Context
            {

                ConnectionString = ConfigurationManager.ConnectionStrings["Core"].ConnectionString,
                Namespace = "NFSolidaria",
                Module = "Core",
                DomainSource = "Core",

                OutputClassDomain = ConfigurationManager.AppSettings["outputClassDomain"],
                OutputClassInfra = ConfigurationManager.AppSettings["outputClassInfra"],
                OutputClassDto = ConfigurationManager.AppSettings["outputClassDto"],
                OutputClassApp = ConfigurationManager.AppSettings["outputClassApp"],
                OutputClassApi = ConfigurationManager.AppSettings["outputClassApi"],
                OutputClassFilter = ConfigurationManager.AppSettings["outputClassFilter"],

                ClearAllFiles = false,
                MakeNavigationPropertys = false,
                DeleteFilesNotFoundTable = false,

                TableInfo = new UniqueListTableInfo
                {
                    new TableInfo { TableName = "Cadastrador", MakeDomain = true, MakeApp = true, MakeDto = true, MakeCrud = true, MakeApi = true },
                    new TableInfo { TableName = "Cupom", MakeDomain = true, MakeApp = true, MakeDto = true, MakeCrud = true, MakeApi = true },
                    new TableInfo { TableName = "Entidade", MakeDomain = true, MakeApp = true, MakeDto = true, MakeCrud = true, MakeApi = true },
                    new TableInfo { TableName = "EntidadeCadastrador", MakeDomain = true, MakeApp = true, MakeDto = true, MakeCrud = true, MakeApi = true },
                    new TableInfo { TableName = "Usuario", MakeDomain = true, MakeApp = true, MakeDto = true, MakeCrud = true, MakeApi = true },
                    new TableInfo { TableName = "UsuarioEntidadeFavorita", MakeDomain = true, MakeApp = true, MakeDto = true, MakeCrud = true, MakeApi = true },
                }

            };
        }


        public IEnumerable<Context> GetConfigContext()
        {

            return new List<Context>
            {
                ConfigContextEtl()
            };

        }

    }
}