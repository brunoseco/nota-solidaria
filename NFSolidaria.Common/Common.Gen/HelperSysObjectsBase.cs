using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Mapping;
using Common.Infrastructure.Log;
using System.Reflection;
using System.Data.Entity;

namespace Common.Gen
{
    public abstract class HelperSysObjectsBase
    {
        protected SqlConnection conn;

        protected string[] auditFields = new string[] { "UserCreateId", "UserCreateDate", "UserAlterId", "UserAlterDate" };

        public IEnumerable<Context> Contexts { get; set; }

        protected string MakeClassNameTemplateDefault(string tableName)
        {
            var result = tableName.Substring(7);
            result = CamelCaseTransform(result);
            result = ClearEnd(result);
            return result;
        }

        protected abstract string makeAuditRow(TableInfo tableInfo, bool generateAudit, Info item, string textTemplateAudit);

        protected abstract bool ExistsAuditFields(IEnumerable<Info> infos);

        protected abstract bool IsAuditField(string field);

        protected void ExecuteTemplateApp(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputApp(tableInfo, configContext);
            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeApp)
                return;


            var resultMakeapp = DefineTemplate.DefineTemplateApp(tableInfo);
            var pathTemplateAppClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeapp);

            var TextTemplateAppClass = File.ReadAllText(pathTemplateAppClass);
            var classBuilder = MakeClassBuilder(tableInfo, configContext, TextTemplateAppClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }


        protected void ExecuteTemplateAppPartial(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputAppPartial(tableInfo, configContext);

            if (!tableInfo.MakeApp)
                return;

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarAppPartialExistentes"]) == false)
                return;

            var resultMakeapp = DefineTemplate.DefineTemplateAppPartial(tableInfo);
            var pathTemplateAppClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeapp);


            var TextTemplateAppClass = File.ReadAllText(pathTemplateAppClass);
            var classBuilder = MakeClassBuilder(tableInfo, configContext, TextTemplateAppClass);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }


        private string MakeClassBuilder(TableInfo tableInfo, Context configContext, string textTemplateClass)
        {
            var classBuilder = textTemplateClass;

            var IDomain = tableInfo.MakeCrud ? "IDomainCrud" : "IDomain";

            classBuilder = classBuilder.Replace("<#namespaceRoot#>", configContext.NamespaceRoot);
            classBuilder = classBuilder.Replace("<#namespace#>", configContext.Namespace);
            classBuilder = classBuilder.Replace("<#domainSource#>", configContext.DomainSource);
            classBuilder = classBuilder.Replace("<#namespaceDomainSource#>", configContext.NamespaceDomainSource);
            classBuilder = classBuilder.Replace("<#className#>", tableInfo.ClassName);
            classBuilder = classBuilder.Replace("<#inheritClassName#>", tableInfo.InheritClassName);
            classBuilder = classBuilder.Replace("<#boundedContext#>", tableInfo.BoundedContext);
            classBuilder = classBuilder.Replace("<#KeyName#>", tableInfo.Keys != null ? tableInfo.Keys.FirstOrDefault() : string.Empty);
            classBuilder = classBuilder.Replace("<#KeyType#>", tableInfo.KeysTypes != null ? tableInfo.KeysTypes.FirstOrDefault() : string.Empty);
            classBuilder = classBuilder.Replace("<#module#>", configContext.Module);
            classBuilder = classBuilder.Replace("<#IDomain#>", IDomain);
            classBuilder = classBuilder.Replace("<#KeyNames#>", MakeKeysFromGetContext(tableInfo));
            classBuilder = classBuilder.Replace("<#tablename#>", tableInfo.TableName);

            classBuilder = MakeReletedNamespace(tableInfo, configContext, classBuilder);


            return classBuilder;
        }

        protected void ExecuteTemplateApi(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {

            var pathOutput = PathOutput.PathOutputApi(tableInfo, configContext);
            if (!tableInfo.MakeApi)
                return;

            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarApiExistentes"]) == false)
                return;

            var resultMakeapi = DefineTemplate.DefineTemplateApi(tableInfo);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeapi);
            var pathTemplateApiGet = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "api.get");


            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplateApiGet = File.ReadAllText(pathTemplateApiGet);

            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            var classBuilderApiGet = string.Empty;

            if (!tableInfo.IsCompositeKey)
            {
                classBuilderApiGet = TextTemplateApiGet;
                classBuilderApiGet = classBuilderApiGet.Replace("<#className#>", tableInfo.ClassName);
                classBuilderApiGet = classBuilderApiGet.Replace("<#inheritClassName#>", tableInfo.InheritClassName);
                classBuilderApiGet = classBuilderApiGet.Replace("<#KeyName#>", tableInfo.Keys.FirstOrDefault());
                classBuilderApiGet = classBuilderApiGet.Replace("<#KeyType#>", tableInfo.KeysTypes.FirstOrDefault());
            }


            classBuilder = classBuilder.Replace("<#ApiGet#>", classBuilderApiGet);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

      

        protected void ExecuteTemplateFilter(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.InheritQuery)
                return;

            var pathOutput = PathOutput.PathOutputFilter(tableInfo, configContext);
            var resultMakefilter = "filter";
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakefilter);
            var pathTemplatePropertys = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.property");

            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplatePropertys = File.ReadAllText(pathTemplatePropertys);


            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            var classBuilderPropertys = string.Empty;

            foreach (var item in infos)
            {
                classBuilderPropertys = MakeFilterDateRange(TextTemplatePropertys, classBuilderPropertys, item);

                if (item.Type == "bool")
                    classBuilderPropertys = AddPropertyFilter(TextTemplatePropertys, classBuilderPropertys, item, item.PropertyName, "bool?");
                else
                    classBuilderPropertys = AddPropertyFilter(TextTemplatePropertys, classBuilderPropertys, item, item.PropertyName, item.Type);

            }

            classBuilder = classBuilder.Replace("<#property#>", classBuilderPropertys);

            using (var stream = new StreamWriter(pathOutput))
            {

                stream.Write(classBuilder);
            }

        }

        private string MakeFilterDateRange(string TextTemplatePropertys, string classBuilderPropertys, Info item)
        {
            if (item.Type == "DateTime" || item.Type == "DateTime?")
            {
                classBuilderPropertys = AddPropertyFilter(TextTemplatePropertys, classBuilderPropertys, item, string.Format("{0}Start", item.PropertyName), item.Type);
                classBuilderPropertys = AddPropertyFilter(TextTemplatePropertys, classBuilderPropertys, item, string.Format("{0}End", item.PropertyName), item.Type);
            }
            return classBuilderPropertys;
        }

        private static bool IsNotDataRage(string propertyName)
        {
            return !propertyName.Contains("Inicio") &&
                                    !propertyName.Contains("Start") &&
                                    !propertyName.Contains("End") &&
                                    !propertyName.Contains("Fim");
        }

        private bool IsNotAuditField(string propertyName)
        {
            return !IsAuditField(propertyName);
        }

        private string AddPropertyFilter(string TextTemplatePropertys, string classBuilderPropertys, Info item, string propertyName, string type)
        {
            var itempropert = TextTemplatePropertys.
                    Replace("<#type#>", type).
                    Replace("<#propertyName#>", propertyName);

            classBuilderPropertys += string.Format("{0}{1}{2}", TabModels(), itempropert, System.Environment.NewLine);
            return classBuilderPropertys;
        }

        protected void ExecuteTemplateFilterPartial(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputFilterPartial(tableInfo, configContext);

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarFiltersPartialExistentes"]) == false)
                return;

            var resultMakefilter = DefineTemplate.DefineTemplateFiltersPartial(tableInfo);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakefilter);

            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);


            using (var stream = new StreamWriter(pathOutput))
            {

                stream.Write(classBuilder);
            }

        }



        protected void ExecuteTemplateDto(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (!tableInfo.MakeDto)
                return;

            var pathOutput = PathOutput.PathOutputDto(tableInfo, configContext);


            if ((File.Exists(pathOutput) && tableInfo.CodeCustomImplemented) || (File.Exists(pathOutput) && tableInfo.InheritQuery))
                return;

            var resultMakedto = tableInfo.InheritQuery ? "inherit\\dto.inherit" : "dto";
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakedto);
            var pathTemplatePropertys = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.property");


            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplatePropertys = File.ReadAllText(pathTemplatePropertys);


            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            var classBuilderPropertys = string.Empty;


            foreach (var item in infos)
            {
                if (auditFields.Contains(item.PropertyName))
                    continue;

                if (item.IsKey == 1)
                {
                    classBuilder = classBuilder.Replace("<#KeyName#>", item.PropertyName);
                    var cast = item.Type == "string" ? ".ToString()" : string.Empty;
                    classBuilder = classBuilder.Replace("<#toString()#>", cast);
                    var expressionInclusion = item.Type == "string" ? string.Format("string.IsNullOrEmpty(this.{0})", item.PropertyName) : string.Format("this.{0} == 0", item.PropertyName);
                }

                var itempropert = TextTemplatePropertys.
                        Replace("<#type#>", item.Type).
                        Replace("<#propertyName#>", item.PropertyName);

                classBuilderPropertys += string.Format("{0}{1}{2}", TabModels(), itempropert, System.Environment.NewLine);

            }

            classBuilder = classBuilder.Replace("<#property#>", classBuilderPropertys);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateDtoSpecialized(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputDtoSpecialized(tableInfo, configContext);

            if (!tableInfo.MakeDto)
                return;


            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarDtoEspecializadosExistentes"]) == false)
                return;


            var resultMakedto = tableInfo.InheritQuery ? "inherit\\dto.specialized.inherit" : "dto.specialized";
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakedto);
            var pathTemplateNavPropertysCollection = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "dto.nav.property.collection");
            var pathTemplateNavPropertysInstance = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "dto.nav.property.instance");


            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplateNavPropertysCollections = File.ReadAllText(pathTemplateNavPropertysCollection);
            var TextTemplateNavPropertysInstance = File.ReadAllText(pathTemplateNavPropertysInstance);


            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            var classBuilderPropertys = string.Empty;

            classBuilder = MakePropertyNavigationDto(tableInfo, configContext, TextTemplateNavPropertysCollections, TextTemplateNavPropertysInstance, classBuilder);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateDtoSpecializedResult(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputDtoSpecializedResult(tableInfo, configContext);

            if (!tableInfo.MakeDto)
                return;


            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarDtoEspecializadosDeResultadoExistentes"]) == false)
                return;


            var resultMakedto = tableInfo.InheritQuery ? "inherit\\dto.specialized.result.inherit" : "dto.specialized.result";
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakedto);
            var pathTemplateNavPropertysCollection = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "dto.nav.property.collection");
            var pathTemplateNavPropertysInstance = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "dto.nav.property.instance");


            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplateNavPropertysCollections = File.ReadAllText(pathTemplateNavPropertysCollection);
            var TextTemplateNavPropertysInstance = File.ReadAllText(pathTemplateNavPropertysInstance);


            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            var classBuilderPropertys = string.Empty;

            classBuilder = MakePropertyNavigationDto(tableInfo, configContext, TextTemplateNavPropertysCollections, TextTemplateNavPropertysInstance, classBuilder);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateAutoMapper(TableInfo tableInfo, Context configContext)
        {

            if (!tableInfo.MakeApp)
                return;

            var pathOutput = PathOutput.PathOutputAutoMapper(configContext);
            if (File.Exists(pathOutput))
                return;
            
            var template = "automapper";
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", template);
            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);
            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateApiConfig(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeApi)
                return;

            
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["GerarWebApiConfig"]) == false)
                return;

            var pathOutput = PathOutput.PathOutputWebApiConfig(configContext);
            if (File.Exists(pathOutput))
                return;


            var pathTemplateClass = DefineTemplate.DefineTemplateApiStart(tableInfo);
            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

     

        protected void ExecuteConfigDomain(TableInfo tableInfo, Context configContext)
        {

            var pathOutput = PathOutput.PathOutputConfigDomain(configContext);
            if (File.Exists(pathOutput))
                return;


            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "config.domain");
            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateCustomFilters(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.InheritQuery)
                return;

            var pathOutput = PathOutput.PathOutputCustomFilters(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;


            var pathTemplateClass = DefineTemplate.DefineTemplateCustomFilters();
            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

      

        protected void ExecuteTemplateMapperProfile(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeDto)
                return;

            var pathOutput = PathOutput.PathOutputAutoMapperProfile(configContext, tableInfo);
            var template = DefineTemplate.DefineTemplateMapperProfile(tableInfo);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", template);
            var pathTemplateMappers = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "profile.registers");
            var pathTemplateMappersSpecilize = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "profile.registers.specilize");
            var pathTemplateMappersSpecilizeResult = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "profile.registers.specilize.result");

            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplateMappers = File.ReadAllText(pathTemplateMappers);
            var TextTemplateMappersSpecilize = File.ReadAllText(pathTemplateMappersSpecilize);
            var TextTemplateMappersSpecilizeResult = File.ReadAllText(pathTemplateMappersSpecilizeResult);

            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            var classBuilderMappers = string.Empty;


            foreach (var item in configContext.TableInfo)
            {
                var className = item.ClassName;

                if (!string.IsNullOrEmpty(className))
                {
                    var itemMappaer = TextTemplateMappers.
                            Replace("<#className#>", className);

                    var itemMappaerSpecilize = TextTemplateMappersSpecilize.
                           Replace("<#className#>", className);

                    var itemMappaerSpecilizeResult = TextTemplateMappersSpecilizeResult.
                        Replace("<#className#>", className);
                    
                    classBuilderMappers += string.Format("{0}{1}{2}", TabSets(), itemMappaer, System.Environment.NewLine);
                    classBuilderMappers += string.Format("{0}{1}{2}", TabSets(), itemMappaerSpecilize, System.Environment.NewLine);
                    classBuilderMappers += string.Format("{0}{1}{2}", TabSets(), itemMappaerSpecilizeResult, System.Environment.NewLine);
                }
            }



            classBuilder = classBuilder.Replace("<#registers#>", classBuilderMappers);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

      

        protected void ExecuteTemplateUri(TableInfo tableInfo, Context configContext)
        {

            var pathOutput = PathOutput.PathOutputUri(tableInfo, configContext);

            if (!tableInfo.MakeApi)
                return;

            var resultMakeUri = tableInfo.InheritQuery ? "inherit\\uri.inherit" : "uri";
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeUri);

            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        public void TemplateDbContextGenerateViews(Context configContext)
        {
            var pathOutput = PathOutput.PathOutputPreCompiledView(configContext);
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["GerarPreCompilacaoDoContexto"]) == false)
                return;

            if (configContext.OutputClassInfra.IsNullOrEmpty())
                return;

            var ctx = LoadDbContext(configContext);
            if (ctx.IsNull())
                return;


            var objectContext = ((IObjectContextAdapter)ctx).ObjectContext;
            var mappingCollection = (StorageMappingItemCollection)objectContext
                .MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);

            var errors = new List<EdmSchemaError>();
            var generateViews = mappingCollection.GenerateViews(errors);
            var computeMappingHashValue = mappingCollection.ComputeMappingHashValue();

            var pathMain = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "precompiledView.main");
            var pathConditional = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "precompiledView.conditional");
            var pathViews = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "precompiledView.view");

            var templateMain = System.IO.File.ReadAllText(pathMain);
            var templateConditional = System.IO.File.ReadAllText(pathConditional);
            var templateView = System.IO.File.ReadAllText(pathViews);

            var makeClassCondidional = string.Empty;
            var makeClassviews = string.Empty;

            foreach (var item in generateViews)
            {

                var viewNameConditional = string.Format("{0}.{1}", item.Key.EntityContainer.Name, item.Key.Name);
                var viewNameMethod = string.Format("{0}_{1}", item.Key.EntityContainer.Name, item.Key.Name);

                makeClassCondidional += TabItemMethod() + templateConditional
                    .Replace("<#viewName#>", viewNameConditional)
                    .Replace("<#viewNameMethod#>", viewNameMethod) + System.Environment.NewLine;


                makeClassviews += templateView
                    .Replace("<#viewNameMethod#>", viewNameMethod)
                    .Replace("<#viewSql#>", item.Value.EntitySql) + System.Environment.NewLine;

            }

            templateMain = templateMain.Replace("<#ComputeMappingHashValue#>", computeMappingHashValue);
            templateMain = templateMain.Replace("<#conditional#>", makeClassCondidional);
            templateMain = templateMain.Replace("<#viewsList#>", makeClassviews).Replace("<#module#>", configContext.Module);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(templateMain);
            }

        }

        private static DbContext LoadDbContext(Context configContext)
        {
            try
            {
                var dll = configContext.OutputClassInfra.Split('\\').LastOrDefault();
                var log = FactoryLog.GetInstace();
                var pathOutputDbContext = string.Format(@"{0}\bin\Debug\{1}.dll", configContext.OutputClassInfra, dll);
                var assembly = Assembly.LoadFrom(pathOutputDbContext);
                var className = string.Format("DbContext{0}", configContext.Module);
                var type = assembly.GetTypes().Where(_ => _.Name == className).SingleOrDefault();
                var instanceCtx = Activator.CreateInstance(type, log);
                var ctx = instanceCtx as DbContext;
                return ctx;
            }
            catch
            {
                return null;
            }
        }

        protected void ExecuteTemplateDbContext(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeDomain)
                return;

            if (tableInfo.InheritQuery)
                return;

            var pathOutput = PathOutput.PathOutputDbContext(configContext);
            var resultMakeContext = "context";
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeContext);
            var pathTemplateRegister = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "context.mappers");


            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplateMappers = File.ReadAllText(pathTemplateRegister);
            if (configContext.Module.IsNullOrEmpty())
                textTemplateClass = textTemplateClass.Replace("<#module#>", configContext.ProjectName);

            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);


            var classBuilderMappers = string.Empty;


            foreach (var item in configContext.TableInfo)
            {

                var itemMappaer = TextTemplateMappers.
                        Replace("<#className#>", item.ClassName);

                classBuilderMappers += string.Format("{0}{1}{2}", TabMaps(), itemMappaer, System.Environment.NewLine);

            }

            classBuilder = classBuilder.Replace("<#mappers#>", classBuilderMappers);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateContainer(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.MakeApp)
                return;

            var pathOutput = PathOutput.PathOutputContainer(configContext);

            var resultMakeContext = tableInfo.InheritQuery ? "inherit\\container.inherit" : "container";
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeContext);
            var pathTemplateInjections = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "container.injections");


            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplateInjections = File.ReadAllText(pathTemplateInjections);

            if (configContext.Module.IsNullOrEmpty())
                textTemplateClass = textTemplateClass.Replace("<#domainSource#>", configContext.ProjectName);
            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            var classBuilderMappers = string.Empty;

            foreach (var item in configContext.TableInfo)
            {
                if (!string.IsNullOrEmpty(item.ClassName))
                {
                    var itemInjections = TextTemplateInjections.
                            Replace("<#namespace#>", configContext.Namespace).
                            Replace("<#module#>", configContext.Module.IsNullOrEmpty() ? configContext.ProjectName : configContext.Module).
                            Replace("<#className#>", item.ClassName).
                            Replace("<#domainSource#>", configContext.DomainSource.IsNullOrEmpty() ? configContext.ProjectName : configContext.DomainSource).
                            Replace("<#namespaceDomainSource#>", configContext.NamespaceDomainSource);


                    classBuilderMappers += string.Format("{0}{1}{2}", TabModels(), itemInjections, System.Environment.NewLine);
                }
            }


            classBuilder = classBuilder.Replace("<#injections#>", classBuilderMappers);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateContainerPartial(TableInfo tableInfo, Context configContext)
        {

            var pathOutput = PathOutput.PathOutputContainerPartial(configContext);

            if (!tableInfo.MakeApp)
                return;

            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarContainerClassPartialExistentes"]) == false)
                return;

            var resultMakeContext = "container.partial";
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeContext);


            var textTemplateClass = File.ReadAllText(pathTemplateClass);


            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateAppTests(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputApplicationTest(tableInfo, configContext);

            if (!tableInfo.MakeTest)
                return;

            if (!tableInfo.MakeApp)
                return;

            var resultMakeAppTest = tableInfo.MakeCrud ? "app.test.crud" : "app.test";


            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeAppTest);

            var textTemplateClass = File.ReadAllText(pathTemplateClass);

            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);
            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateAppTestsMoq(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {


            if (!tableInfo.MakeApp)
                return;

            var resultMakeAppTestmoq = "app.test.moq";


            var pathOutput = PathOutput.PathOutputApplicationTestMoq(tableInfo, configContext);

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeAppTestmoq);
            var pathTemplateMoqValues = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "app.test.moqvalues");

            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplateMoqValues = File.ReadAllText(pathTemplateMoqValues);

            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            var classBuilderMoqValues = string.Empty;

            foreach (var item in infos)
            {

                if (item.IsKey == 1)
                    continue;

                if (IsAuditField(item.PropertyName))
                    continue;

                var itemvalue = TextTemplateMoqValues.
                        Replace("<#propertyName#>", item.PropertyName).
                        Replace("<#length#>", IsString(item) && IsNotVarcharMax(item) ? item.Length : string.Empty).
                        Replace("<#moqMethod#>", DefineMoqMethd(item.Type));

                classBuilderMoqValues += string.Format("{0}{1}", itemvalue, System.Environment.NewLine);

            }

            classBuilder = classBuilder.Replace("<#moqValuesinsert#>", classBuilderMoqValues);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateAppTestsMoqPartial(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputApplicationTestMoqPartial(tableInfo, configContext);

            if (!tableInfo.MakeApp)
                return;

            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarMoqClassPartialExistentes"]) == false)
                return;

            var resultMakeAppTest = "app.test.moq.partial";
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeAppTest);
            var pathTemplateReletedValues = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "app.test.reletedvalues");


            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplateReletedValues = File.ReadAllText(pathTemplateReletedValues);

            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);
            classBuilder = MakeReletedIntanceValues(tableInfo, configContext, TextTemplateReletedValues, classBuilder);

            var classBuilderMoqValues = string.Empty;


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateApiTests(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputApiTest(tableInfo, configContext);

            if (!tableInfo.MakeTest)
                return;

            if (!tableInfo.MakeApi)
                return;


            var resultMakeAppTest = tableInfo.MakeCrud ? "api.test.crud" : "api.test";
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeAppTest);
            var pathTemplateMoqValues = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "api.test.moqvalues");


            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplateMoqValues = File.ReadAllText(pathTemplateMoqValues);


            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            var classBuilderMoqValues = string.Empty;

            foreach (var item in infos)
            {

                if (item.IsKey == 1)
                    continue;

                if (IsAuditField(item.PropertyName))
                    continue;

                var itemvalue = TextTemplateMoqValues.
                        Replace("<#propertyName#>", item.PropertyName).
                        Replace("<#length#>", item.Type == "string" ? item.Length : string.Empty).
                        Replace("<#moqMethod#>", DefineMoqMethd(item.Type));

                classBuilderMoqValues += string.Format("{0}{1}{2}", TabSets(), itemvalue, System.Environment.NewLine);

            }

            classBuilder = classBuilder.Replace("<#moqValuesinsert#>", classBuilderMoqValues);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateApiTestsBasic(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = PathOutput.PathOutputApiTest(tableInfo, configContext);

            if (File.Exists(pathOutput))
                return;

            if (!tableInfo.MakeTest)
                return;

            if (!tableInfo.MakeApi)
                return;


            var resultMakeAppTest = "api.test";
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeAppTest);
            var textTemplateClass = File.ReadAllText(pathTemplateClass);

            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            classBuilder = classBuilder.Replace("<#filterByModel#>", MakeKFilterByModel(tableInfo));

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteFixModels(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;

            var methodExpression = "public Expression<Func<<#className#>, object>>[] DataAgregation(Filter filter)";
            var methodExpressionBuilder = MakeClassBuilder(tableInfo, configContext, methodExpression);
            var pathInputDomainModelsPartial = PathOutput.PathOutputDomainModelsPartial(tableInfo, configContext);
            var method = ExtractMethod(methodExpressionBuilder, pathInputDomainModelsPartial);

            //var PathOutputDomainModelsCustom = PathOutput.PathOutputDomainModelsCustom(tableInfo, configContext);
            //File.AppendAllText(PathOutputDomainModelsCustom,method);
        }

        private static string ExtractMethod(string methodExpression, string pathModelInput)
        {
            var method = string.Empty;
            if (File.Exists(pathModelInput))
            {
                var contentClassLines = File.ReadAllLines(pathModelInput);
                var qtdChavesAbertas = 0;
                var qtdChavesFechadas = 0;
                var methodFound = false;
                foreach (var line in contentClassLines)
                {
                    var lineTrim = line.TrimStart().TrimEnd();
                    methodFound = !methodFound ? lineTrim == methodExpression : methodFound;
                    if (methodFound)
                    {
                        if (lineTrim == "{") qtdChavesAbertas++;
                        if (lineTrim == "}") qtdChavesFechadas++;

                        method += line;

                        if (qtdChavesAbertas > 0 && qtdChavesFechadas > 0 && qtdChavesAbertas == qtdChavesFechadas)
                            break;

                    }

                }
            }
            return method;
        }


        protected void ExecuteTemplateSimpleFiltersInherit(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {

            if (!tableInfo.InheritQuery)
                return;

            var pathOutput = PathOutput.PathOutputSimpleFilters(tableInfo, configContext);
            var resultMakeCrud = DefineTemplate.DefineTemplateSimpleFilters(tableInfo);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeCrud);
            var pathTemplateFilters = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.filters.expression");

            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var textTemplateFilters = File.ReadAllText(pathTemplateFilters);

            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);
            var classBuilderFilters = string.Empty;

            foreach (var item in infos)
            {


                var itemFilters = string.Empty;

                if (item.Type == "string")
                {
                    itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                    itemFilters = itemFilters.Replace("<#condition#>", string.Format("_=>_.{0}.Contains(filters.{0})", item.PropertyName));
                    itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                }
                else if (item.Type == "DateTime")
                {
                    var itemFiltersStart = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}Start", item.PropertyName));
                    itemFiltersStart = itemFiltersStart.Replace("<#condition#>", string.Format("_=>_.{0} >= filters.{0}Start ", item.PropertyName));
                    itemFiltersStart = itemFiltersStart.Replace("<#filtersRange#>", string.Empty);

                    var itemFiltersEnd = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}End", item.PropertyName));
                    itemFiltersEnd = itemFiltersEnd.Replace("<#condition#>", string.Format("_=>_.{0}  <= filters.{0}End", item.PropertyName));
                    itemFiltersEnd = itemFiltersEnd.Replace("<#filtersRange#>", string.Format("filters.{0}End = filters.{0}End.AddDays(1).AddMilliseconds(-1);", item.PropertyName));

                    itemFilters = String.Format("{0}{1}{2}{3}{4}", itemFiltersStart, System.Environment.NewLine, TabSets(), itemFiltersEnd, System.Environment.NewLine);

                }
                else if (item.Type == "DateTime?")
                {
                    var itemFiltersStart = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}Start", item.PropertyName));
                    itemFiltersStart = itemFiltersStart.Replace("<#condition#>", string.Format("_=>_.{0} != null && _.{0}.Value >= filters.{0}Start.Value", item.PropertyName));
                    itemFiltersStart = itemFiltersStart.Replace("<#filtersRange#>", string.Empty);

                    var itemFiltersEnd = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}End", item.PropertyName));
                    itemFiltersEnd = itemFiltersEnd.Replace("<#condition#>", string.Format("_=>_.{0} != null &&  _.{0}.Value <= filters.{0}End", item.PropertyName));
                    itemFiltersEnd = itemFiltersEnd.Replace("<#filtersRange#>", string.Format("filters.{0}End = filters.{0}End.Value.AddDays(1).AddMilliseconds(-1);", item.PropertyName));

                    itemFilters = String.Format("{0}{1}{2}{3}{4}", itemFiltersStart, System.Environment.NewLine, TabSets(), itemFiltersEnd, System.Environment.NewLine);

                }
                else if (item.Type == "bool?")
                {
                    itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                    itemFilters = itemFilters.Replace("<#condition#>", string.Format("_=>_.{0} != null && _.{0}.Value == filters.{0}", item.PropertyName));
                    itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                }
                else if (item.Type == "int?" || item.Type == "Int64?" || item.Type == "Int16?" || item.Type == "decimal?" || item.Type == "float?")
                {
                    itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                    itemFilters = itemFilters.Replace("<#condition#>", string.Format("_=>_.{0} != null && _.{0}.Value == filters.{0}", item.PropertyName));
                    itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                }
                else
                {
                    itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                    itemFilters = itemFilters.Replace("<#condition#>", string.Format("_=>_.{0} == filters.{0}", item.PropertyName));
                    itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                }


                classBuilderFilters += string.Format("{0}{1}{2}", TabSets(), itemFilters, System.Environment.NewLine);

            }

            classBuilder = classBuilder.Replace("<#filtersExpressions#>", classBuilderFilters);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateModels(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.CodeCustomImplemented)
                return;

            if (!tableInfo.MakeDomain)
                return;

            if (tableInfo.InheritQuery)
                return;

            var pathOutput = PathOutput.PathOutputDomainModels(tableInfo, configContext);
            var resultMakeCrud = DefineTemplate.DefineTemplateModels(tableInfo);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeCrud);
            var pathTemplatePropertys = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.property");
            var pathTemplateFilters = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.filters.expression");
            var pathTemplateAuditCall = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.audit.call");

            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var textTemplatePropertys = File.ReadAllText(pathTemplatePropertys);
            var textTemplateFilters = File.ReadAllText(pathTemplateFilters);
            var TextTemplateAuditCall = File.ReadAllText(pathTemplateAuditCall);

            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);
            classBuilder = classBuilder.Replace("<#WhereSingle#>", MakeKeysFromGet(tableInfo));
            classBuilder = ToolName(tableInfo, classBuilder);

            var classBuilderPropertys = string.Empty;
            var classBuilderFilters = string.Empty;
            var textTemplateAudit = string.Empty;

            var generateAudit = ExistsAuditFields(infos);

            foreach (var item in infos)
            {
                if (item.IsKey == 1)
                {
                    classBuilder = classBuilder.
                        Replace("<#KeyName#>", item.PropertyName).
                        Replace("<#KeyNameType#>", item.Type);

                    var cast = item.Type == "string" ? ".ToString()" : string.Empty;
                    classBuilder = classBuilder.Replace("<#toString()#>", cast);
                }

                var itemFilters = string.Empty;

                if (item.Type == "string")
                {
                    itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                    itemFilters = itemFilters.Replace("<#condition#>", string.Format("_=>_.{0}.Contains(filters.{0})", item.PropertyName));
                    itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                }
                else if (item.Type == "DateTime")
                {
                    var itemFiltersStart = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}Start", item.PropertyName));
                    itemFiltersStart = itemFiltersStart.Replace("<#condition#>", string.Format("_=>_.{0} >= filters.{0}Start ", item.PropertyName));
                    itemFiltersStart = itemFiltersStart.Replace("<#filtersRange#>", string.Empty);

                    var itemFiltersEnd = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}End", item.PropertyName));
                    itemFiltersEnd = itemFiltersEnd.Replace("<#condition#>", string.Format("_=>_.{0}  <= filters.{0}End", item.PropertyName));
                    itemFiltersEnd = itemFiltersEnd.Replace("<#filtersRange#>", string.Format("filters.{0}End = filters.{0}End.AddDays(1).AddMilliseconds(-1);", item.PropertyName));

                    itemFilters = String.Format("{0}{1}{2}{3}{4}", itemFiltersStart, System.Environment.NewLine, TabSets(), itemFiltersEnd, System.Environment.NewLine);

                }
                else if (item.Type == "DateTime?")
                {
                    var itemFiltersStart = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}Start", item.PropertyName));
                    itemFiltersStart = itemFiltersStart.Replace("<#condition#>", string.Format("_=>_.{0} != null && _.{0}.Value >= filters.{0}Start.Value", item.PropertyName));
                    itemFiltersStart = itemFiltersStart.Replace("<#filtersRange#>", string.Empty);

                    var itemFiltersEnd = textTemplateFilters.Replace("<#propertyName#>", String.Format("{0}End", item.PropertyName));
                    itemFiltersEnd = itemFiltersEnd.Replace("<#condition#>", string.Format("_=>_.{0} != null &&  _.{0}.Value <= filters.{0}End", item.PropertyName));
                    itemFiltersEnd = itemFiltersEnd.Replace("<#filtersRange#>", string.Format("filters.{0}End = filters.{0}End.Value.AddDays(1).AddMilliseconds(-1);", item.PropertyName));

                    itemFilters = String.Format("{0}{1}{2}{3}{4}", itemFiltersStart, System.Environment.NewLine, TabSets(), itemFiltersEnd, System.Environment.NewLine);

                }
                else if (item.Type == "bool?")
                {
                    itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                    itemFilters = itemFilters.Replace("<#condition#>", string.Format("_=>_.{0} != null && _.{0}.Value == filters.{0}", item.PropertyName));
                    itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                }
                else if (item.Type == "int?" || item.Type == "Int64?" || item.Type == "Int16?" || item.Type == "decimal?" || item.Type == "float?")
                {
                    itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                    itemFilters = itemFilters.Replace("<#condition#>", string.Format("_=>_.{0} != null && _.{0}.Value == filters.{0}", item.PropertyName));
                    itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                }
                else
                {
                    itemFilters = textTemplateFilters.Replace("<#propertyName#>", item.PropertyName);
                    itemFilters = itemFilters.Replace("<#condition#>", string.Format("_=>_.{0} == filters.{0}", item.PropertyName));
                    itemFilters = itemFilters.Replace("<#filtersRange#>", string.Empty);
                }


                classBuilderFilters += string.Format("{0}{1}{2}", TabSets(), itemFilters, System.Environment.NewLine);

                var itempropert = textTemplatePropertys.
                        Replace("<#type#>", item.Type).
                        Replace("<#propertyName#>", item.PropertyName);
                classBuilderPropertys += string.Format("{0}{1}{2}", TabModels(), itempropert, System.Environment.NewLine);

                textTemplateAudit = makeAuditRow(tableInfo, generateAudit, item, textTemplateAudit);

            }


            classBuilder = classBuilder.Replace("<#callAudit#>", generateAudit ? TextTemplateAuditCall : string.Empty);
            classBuilder = classBuilder.Replace("<#audit#>", textTemplateAudit);
            classBuilder = classBuilder.Replace("<#IAudit#>", generateAudit ? " IAudit, " : string.Empty);

            classBuilder = classBuilder.Replace("<#property#>", classBuilderPropertys);

            classBuilder = classBuilder.Replace("<#filtersExpressions#>", classBuilderFilters);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }


        private static string ToolName(TableInfo tableInfo, string classBuilder)
        {
            classBuilder = classBuilder.Replace("<#toolName#>", !string.IsNullOrEmpty(tableInfo.ToolsName) ? string.Format("base.toolName = {0};", tableInfo.ToolsName) : string.Format("base.toolName = {0};", "string.Empty"));
            return classBuilder;
        }

        protected void ExecuteTemplateModelsPartial(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (!tableInfo.MakeDomain)
                return;

            var pathOutput = PathOutput.PathOutputDomainModelsPartial(tableInfo, configContext);
            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarModelPartialExistentes"]) == false)
                return;

            var resultMakeCrud = DefineTemplate.DefineTemplateModelPartial(tableInfo);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeCrud);
            var pathTemplateNavPropertysCollection = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.nav.property.collection");
            var pathTemplateNavPropertysInstance = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.nav.property.instance");


            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplateNavPropertysCollections = File.ReadAllText(pathTemplateNavPropertysCollection);
            var TextTemplateNavPropertysInstance = File.ReadAllText(pathTemplateNavPropertysInstance);


            var classBuilderitemTemplateValidation = string.Empty;

            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);
            classBuilder = classBuilder.Replace("<#WhereSingle#>", MakeKeysFromGet(tableInfo));


            foreach (var item in infos)
            {
                if (item.IsKey == 1)
                {
                    classBuilder = classBuilder
                        .Replace("<#KeyName#>", item.PropertyName)
                        .Replace("<#KeyNameType#>", item.Type);

                    var cast = item.Type == "string" ? ".ToString()" : string.Empty;
                    classBuilder = classBuilder.Replace("<#toString()#>", cast);

                }


            }

            classBuilder = classBuilder.Replace("<#property#>", classBuilderitemTemplateValidation);
            classBuilder = MakePropertyNavigationModels(tableInfo, configContext, TextTemplateNavPropertysCollections, TextTemplateNavPropertysInstance, classBuilder);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }



        private static string MakeKeysFromGetContext(TableInfo tableInfo)
        {
            var keys = string.Empty;
            if (tableInfo.Keys != null)
            {
                if (tableInfo.Keys.Count() > 0)
                {
                    foreach (var item in tableInfo.Keys)
                        keys += string.Format("model.{0},", item);

                    keys = keys.Substring(0, keys.Length - 1);
                }
            }
            return keys;
        }

        private static string MakeKeysFromGet(TableInfo tableInfo)
        {
            var keys = string.Empty;
            if (tableInfo.Keys.Count() > 0)
            {
                foreach (var item in tableInfo.Keys)
                    keys += string.Format(".Where(_=>_.{0} == model.{1})", item, item);
            }
            return keys;
        }

        private static string MakeKFilterByModel(TableInfo tableInfo)
        {
            var keys = string.Empty;
            if (tableInfo.Keys.Count() > 0)
            {
                foreach (var item in tableInfo.Keys)
                    keys += string.Format("{0} = first.{1},", item, item);
            }
            return keys;
        }

        protected void ExecuteTemplateModelsValiadation(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.InheritQuery)
                return;

            var pathOutput = PathOutput.PathOutputDomainModelsValidation(tableInfo, configContext);

            if (!tableInfo.MakeDomain)
                return;

            if (!tableInfo.MakeCrud)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.validation");
            var pathTemplatevalidation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.validation.property");
            var pathTemplatevalidationLength = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.validation.length");
            var pathTemplatevalidationRequired = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.validation.required");


            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplateValidation = File.ReadAllText(pathTemplatevalidation);
            var TextTemplateValidationLength = File.ReadAllText(pathTemplatevalidationLength);
            var TextTemplateValidationRequired = File.ReadAllText(pathTemplatevalidationRequired);

            var classBuilderitemTemplateValidation = string.Empty;
            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            foreach (var item in infos)
            {

                if (item.IsKey == 1)
                    continue;

                if (item.PropertyName.EndsWith("Id"))
                    continue;

                classBuilderitemTemplateValidation = MakeValidationsAttributes(TextTemplateValidation, TextTemplateValidationLength, TextTemplateValidationRequired, classBuilderitemTemplateValidation, item);

            }

            classBuilderitemTemplateValidation = classBuilderitemTemplateValidation.Replace("<#className#>", tableInfo.ClassName);
            classBuilder = classBuilder.Replace("<#property#>", classBuilderitemTemplateValidation);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        private string MakeValidationsAttributes(string TextTemplateValidation, string TextTemplateValidationLength, string TextTemplateValidationRequired, string classBuilderitemTemplateValidation, Info item)
        {
            if (IsRequired(item) && IsString(item) && IsNotVarcharMax(item))
            {
                var itemTemplateValidationLegth = TextTemplateValidationLength
                    .Replace("<#Length#>", item.Length)
                    .Replace("<#propertyName#>", item.PropertyName);

                var itemTemplateValidationRequired = TextTemplateValidationRequired
                   .Replace("<#propertyName#>", item.PropertyName);

                var itemTemplateValidation = TextTemplateValidation.Replace("<#propertyName#>", item.PropertyName).Replace("<#tab#>", TabModels());
                itemTemplateValidation = itemTemplateValidation.Replace("<#RequiredValidation#>", TabModels(itemTemplateValidationRequired));
                itemTemplateValidation = itemTemplateValidation.Replace("<#MaxLengthValidation#>", TabModels(itemTemplateValidationLegth));

                classBuilderitemTemplateValidation += string.Format("{0}{1}", itemTemplateValidation, System.Environment.NewLine);

            }

            if (IsRequired(item) && IsNotString(item))
            {
                var itemTemplateValidationLegth = TextTemplateValidationLength
                    .Replace("<#Length#>", item.Length)
                    .Replace("<#propertyName#>", item.PropertyName);


                var itemTemplateValidationRequired = TextTemplateValidationRequired
                   .Replace("<#propertyName#>", item.PropertyName);

                var itemTemplateValidation = TextTemplateValidation.Replace("<#propertyName#>", item.PropertyName).Replace("<#tab#>", TabModels());
                itemTemplateValidation = itemTemplateValidation.Replace("<#RequiredValidation#>", TabModels(itemTemplateValidationRequired));
                itemTemplateValidation = RemoveLine(itemTemplateValidation, "<#MaxLengthValidation#>");

                classBuilderitemTemplateValidation += string.Format("{0}{1}", itemTemplateValidation, System.Environment.NewLine);

            }

            if (!IsRequired(item) && IsString(item) && IsNotVarcharMax(item))
            {
                var itemTemplateValidationLegth = TextTemplateValidationLength
                    .Replace("<#Length#>", item.Length)
                    .Replace("<#propertyName#>", item.PropertyName);

                var itemTemplateValidation = TextTemplateValidation.Replace("<#propertyName#>", item.PropertyName).Replace("<#tab#>", TabModels());
                itemTemplateValidation = RemoveLine(itemTemplateValidation, "<#RequiredValidation#>");
                itemTemplateValidation = itemTemplateValidation.Replace("<#MaxLengthValidation#>", TabModels(itemTemplateValidationLegth));

                classBuilderitemTemplateValidation += string.Format("{0}{1}", itemTemplateValidation, System.Environment.NewLine);

            }
            return classBuilderitemTemplateValidation;
        }

        private static bool IsString(Info item)
        {
            return item.Type == "string";
        }

        private static bool IsNotString(Info item)
        {
            return item.Type != "string";
        }

        private static bool IsNotVarcharMax(Info item)
        {
            return !item.Length.Contains("-1");
        }

        protected void ExecuteTemplateModelsValiadationPartial(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.InheritQuery)
                return;

            var pathOutput = PathOutput.PathOutputDomainModelsValidationPartial(tableInfo, configContext);

            if (!tableInfo.MakeDomain)
                return;

            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarValidationsPartialExistentes"]) == false)
                return;

            if (!tableInfo.MakeCrud)
                return;


            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.partial.validation");
            var pathTemplatevalidation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.validation.property");
            var pathTemplatevalidationLength = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.validation.length");
            var pathTemplatevalidationRequired = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.validation.required");

            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var TextTemplateValidation = File.ReadAllText(pathTemplatevalidation);
            var TextTemplateValidationLength = File.ReadAllText(pathTemplatevalidationLength);
            var TextTemplateValidationRequired = File.ReadAllText(pathTemplatevalidationRequired);

            var classBuilderitemTemplateValidation = string.Empty;
            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            foreach (var item in infos)
            {

                if (item.IsKey == 1)
                    continue;

                if (item.PropertyName.ToLower().EndsWith("id"))
                    classBuilderitemTemplateValidation = MakeValidationsAttributes(TextTemplateValidation, TextTemplateValidationLength, TextTemplateValidationRequired, classBuilderitemTemplateValidation, item);

            }

            classBuilderitemTemplateValidation = classBuilderitemTemplateValidation.Replace("<#className#>", tableInfo.ClassName);
            classBuilder = classBuilder.Replace("<#property#>", classBuilderitemTemplateValidation);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateModelsCustom(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (tableInfo.InheritQuery)
                return;

            var pathOutput = PathOutput.PathOutputDomainModelsCustom(tableInfo, configContext);

            if (!tableInfo.MakeDomain)
                return;

            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarModelCustomExistentes"]) == false)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.custom");
            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateSummary(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = PathOutput.PathOutputSummary(tableInfo, configContext);

            if (!tableInfo.MakeSummary)
                return;

            if (File.Exists(pathOutput) || tableInfo.CodeCustomImplemented)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "summary");
            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateDbContextInherit(TableInfo tableInfo, Context configContext)
        {
            if (!tableInfo.InheritQuery)
                return;

            if (!tableInfo.MakeDomain)
                return;

            var pathOutput = PathOutput.PathOutputContextsInherit(tableInfo, configContext);
            if (File.Exists(pathOutput))
                return;

            var resultMakeContext = "inherit\\context.inherit";
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", resultMakeContext);

            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            if (configContext.Module.IsNullOrEmpty())
                textTemplateClass = textTemplateClass.Replace("<#module#>", configContext.ProjectName);

            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

        protected void ExecuteTemplateMapsPartial(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            var pathOutput = PathOutput.PathOutputMapsPartial(tableInfo, configContext);

            if (!tableInfo.MakeDomain)
                return;

            if (File.Exists(pathOutput) && tableInfo.CodeCustomImplemented)
                return;

            if (File.Exists(pathOutput) && Convert.ToBoolean(ConfigurationManager.AppSettings["GerarMapperPartialExistentes"]) == false)
                return;

            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", DefineTemplate.DefineTemplateMapPartialinherit(tableInfo));
            var textTemplateClass = File.ReadAllText(pathTemplateClass);

            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);


            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }

        }

       

        protected void ExecuteTemplateMaps(TableInfo tableInfo, Context configContext, IEnumerable<Info> infos)
        {
            if (!tableInfo.MakeDomain)
                return;

            var pathOutput = PathOutput.PathOutputMaps(tableInfo, configContext);
            var pathTemplateClass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", DefineTemplate.DefineTemplateMaps(tableInfo));
            var pathTemplateLength = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "maps.length");
            var pathTemplateRequired = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "maps.required");
            var pathTemplateMapper = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "maps.mapper");
            var pathTemplateManyToMany = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "maps.manytomany");
            var pathTemplateCompositeKey = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "maps.compositekey");

            var textTemplateClass = File.ReadAllText(pathTemplateClass);
            var textTemplateLength = File.ReadAllText(pathTemplateLength);
            var textTemplateRequired = File.ReadAllText(pathTemplateRequired);
            var textTemplateMapper = File.ReadAllText(pathTemplateMapper);
            var textTemplateManyToMany = File.ReadAllText(pathTemplateManyToMany);
            var textTemplateCompositeKey = File.ReadAllText(pathTemplateCompositeKey);

            var classBuilderitemTemplateLength = string.Empty;
            var classBuilderitemTemplateRequired = string.Empty;
            var classBuilderitemplateMapper = string.Empty;
            var classBuilderitemplateMapperKey = string.Empty;
            var classBuilderitemplateManyToMany = string.Empty;
            var classBuilderitemplateCompositeKey = string.Empty;


            var classBuilder = MakeClassBuilder(tableInfo, configContext, textTemplateClass);

            classBuilderitemplateCompositeKey = MakeKey(infos, textTemplateCompositeKey, classBuilderitemplateCompositeKey);

            foreach (var item in infos)
            {



                if (IsString(item) && IsNotVarcharMax(item))
                {
                    var itemTemplateLength = textTemplateLength
                        .Replace("<#propertyName#>", item.PropertyName)
                        .Replace("<#length#>", item.Length);

                    classBuilderitemTemplateLength += string.Format("{0}{1}{2}", TabMaps(), itemTemplateLength, System.Environment.NewLine);
                }

                if (item.isNullable == 0)
                {
                    var itemTemplateRequired = textTemplateRequired
                       .Replace("<#propertyName#>", item.PropertyName);

                    classBuilderitemTemplateRequired += string.Format("{0}{1}{2}", TabMaps(), itemTemplateRequired, System.Environment.NewLine);
                }

                if (item.IsKey == 1)
                {
                    var itemplateMapper = textTemplateMapper
                        .Replace("<#propertyName#>", item.PropertyName)
                        .Replace("<#columnName#>", item.ColumnName);

                    classBuilderitemplateMapperKey += string.Format("{0}{1}{2}", TabMaps(), itemplateMapper, System.Environment.NewLine);

                }
                else
                {

                    var itemplateMapper = textTemplateMapper
                       .Replace("<#propertyName#>", item.PropertyName)
                       .Replace("<#columnName#>", item.ColumnName);

                    classBuilderitemplateMapper += string.Format("{0}{1}{2}", TabMaps(), itemplateMapper, System.Environment.NewLine);
                }

            }


            if (!string.IsNullOrEmpty(tableInfo.TableHelper))
            {

                var itemTemplateManyToMany = textTemplateManyToMany
                      .Replace("<#propertyNavigationLeft#>", tableInfo.ClassNameRigth)
                      .Replace("<#propertyNavigationRigth#>", tableInfo.ClassName)
                      .Replace("<#MapLeftKey#>", tableInfo.LeftKey)
                      .Replace("<#MapRightKey#>", tableInfo.RightKey)
                      .Replace("<#TableHelper#>", tableInfo.TableHelper);

                classBuilderitemplateManyToMany += string.Format("{0}{1}{2}", TabMaps(), itemTemplateManyToMany, System.Environment.NewLine);

            }

            classBuilder = classBuilder.Replace("<#IsRequired#>", classBuilderitemTemplateRequired);
            classBuilder = classBuilder.Replace("<#HasMaxLength#>", classBuilderitemTemplateLength);
            classBuilder = classBuilder.Replace("<#Mapper#>", classBuilderitemplateMapper);
            classBuilder = classBuilder.Replace("<#keyName#>", classBuilderitemplateMapperKey);
            classBuilder = classBuilder.Replace("<#ManyToMany#>", classBuilderitemplateManyToMany);
            classBuilder = classBuilder.Replace("<#CompositeKey#>", classBuilderitemplateCompositeKey);

            using (var stream = new StreamWriter(pathOutput))
            {
                stream.Write(classBuilder);
            }


        }


        protected bool Open(string connectionString)
        {
            this.conn = new SqlConnection(connectionString);
            this.conn.Open();
            return true;
        }

        protected string TypeConvertCSharp(string typeSQl, int isNullable)
        {



            switch (typeSQl)
            {
                case "char":
                case "nchar":
                case "nvarchar":
                case "varchar":
                case "text":
                    return "string";
                case "date":
                case "datetime":
                    return isNullable == 1 ? "DateTime?" : "DateTime";
                case "bigint":
                    return isNullable == 1 ? "Int64?" : "Int64";
                case "int":
                    return isNullable == 1 ? "int?" : "int";
                case "bit":
                    return isNullable == 1 ? "bool?" : "bool";
                case "tinyint":
                    return isNullable == 1 ? "byte?" : "byte";
                case "smallint":
                case "int16":
                    return isNullable == 1 ? "Int16?" : "Int16";
                case "numeric":
                case "decimal":
                case "money":
                    return isNullable == 1 ? "decimal?" : "decimal";
                case "float":
                    return isNullable == 1 ? "float?" : "float";
                case "image":
                    return isNullable == 1 ? "byte[]" : "byte[]";



                default:
                    return typeSQl;
            }

        }

        private string DefineMoqMethd(string type)
        {

            switch (type.ToLower())
            {
                case "string":
                case "nchar":
                    return "MakeStringValueSuccess";
                case "int":
                case "int?":
                    return "MakeIntValueSuccess";
                case "decimal":
                case "decimal?":
                case "money":
                    return "MakeDecimalValueSuccess";
                case "float?":
                case "float":
                    return "MakeFloatValueSuccess";
                case "datetime":
                case "datetime?":
                    return "MakeDateTimeValueSuccess";
                case "bool":
                case "bool?":
                    return "MakeBoolValueSuccess";
                default:
                    break;
            }


            throw new InvalidOperationException("tipo não implementado");

        }

        protected static string MakePropertyNameForId(TableInfo tableConfig)
        {
            var propertyName = string.Format("{0}Id", tableConfig.ClassName);
            return propertyName;
        }

        protected string MakePropertyNameDefault(string columnName)
        {

            var newcolumnName = columnName.Substring(4);

            newcolumnName = TranslateNames(newcolumnName);

            newcolumnName = CamelCaseTransform(newcolumnName);

            newcolumnName = ClearEnd(newcolumnName);

            return newcolumnName;

        }

        protected static string ClearEnd(string value)
        {
            value = value.Replace("_X_", "");
            value = value.Replace("_", "");
            value = value.Replace("-", "");
            return value;
        }

        protected static string TranslateNames(string newcolumnName)
        {

            newcolumnName = newcolumnName.Contains("_cd_") ? String.Concat(newcolumnName.Replace("_cd_", "_"), "_Id_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_nm_") ? String.Concat(newcolumnName.Replace("_nm_", "_"), "_Nome_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_ds_") ? String.Concat(newcolumnName.Replace("_ds_", "_"), "_Descricao_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_nr_") ? String.Concat(newcolumnName.Replace("_nr_", "_"), "_Numero_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_pc_") ? String.Concat(newcolumnName.Replace("_pc_", "_"), "_Porcentagem_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_qt_") ? String.Concat(newcolumnName.Replace("_qt_", "_"), "_Quantidade_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_dt_") ? String.Concat(newcolumnName.Replace("_dt_", "_"), "_Data_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_fl_") ? String.Concat(newcolumnName.Replace("_fl_", "_"), "_Flag_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_vl_") ? String.Concat(newcolumnName.Replace("_vl_", "_"), "_Valor_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_in_") ? String.Concat(newcolumnName.Replace("_in_", "_"), "") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_sg_") ? String.Concat(newcolumnName.Replace("_sg_", "_"), "_Sigla_") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_tp_") ? String.Concat(newcolumnName.Replace("_tp_", "_"), "_Tipo_") : newcolumnName;

            return newcolumnName;
        }

        protected static string TranslateNamesPatnerX(string newcolumnName)
        {


            newcolumnName = newcolumnName.Contains("_X_") ? String.Concat(newcolumnName.Replace("_X_", ""), "") : newcolumnName;
            newcolumnName = newcolumnName.Contains("_cd") ? String.Concat(newcolumnName.Replace("_cd", ""), "_Id") : newcolumnName;

            return newcolumnName;
        }

        protected static string CamelCaseTransform(string newcolumnName)
        {

            newcolumnName = string.Concat(newcolumnName
                           .Split('_')
                           .Where(_ => !string.IsNullOrEmpty(_))
                           .Select(y => y.Substring(0, 1).ToUpper() + y.Substring(1) + "_"));



            return newcolumnName;
        }

        protected string TabModels(string value)
        {
            return String.Format("{0}{1}", "        ", value);
        }

        protected string TabModels()
        {
            return TabModels(string.Empty);
        }

        protected string TabMaps()
        {
            return "            ";
        }

        protected string TabSets()
        {
            return "            ";
        }

        protected string TabItemMethod()
        {
            return "        ";
        }

        protected string makePrefixTable(TableInfo tableConfig)
        {
            if (!tableConfig.TableName.Contains("_X_"))
            {
                var prefix = tableConfig.TableName.Split('_')[1];
                return prefix;
            }
            return string.Empty;
        }

        protected string makePrefixField(string columnName)
        {
            return columnName.Split('_')[0];
        }

        protected string RemoveLine(string itemTemplateValidation, string targetField)
        {
            return itemTemplateValidation.Replace(targetField + System.Environment.NewLine, string.Empty);
        }

        protected bool IsRequired(Info item)
        {
            return item.isNullable == 0;
        }

        protected string MakePropertyNavigationDto(TableInfo tableInfo, Context configContext, string TextTemplateNavPropertysCollection, string TextTemplateNavPropertysInstace, string classBuilder)
        {
            var classBuilderNavPropertys = string.Empty;

            foreach (var item in tableInfo.ReletedClasss)
            {



                var TextTemplateNavPropertys = item.NavigationType == NavigationType.Instance ? TextTemplateNavPropertysInstace : TextTemplateNavPropertysCollection;


                if (item.ClassName == tableInfo.ClassName)
                    classBuilderNavPropertys += String.Format("{0}{1}", TabModels(), TextTemplateNavPropertys
                        .Replace("<#className#>", item.ClassName)
                        .Replace("<#classNameNav#>", String.Format("{0}Self", item.ClassName)));

                if (item.ClassName != tableInfo.ClassName)
                    classBuilderNavPropertys += String.Format("{0}{1}", TabModels(), TextTemplateNavPropertys
                        .Replace("<#className#>", item.ClassName)
                        .Replace("<#classNameNav#>", item.ClassName));


            }

            classBuilder = classBuilder
                .Replace("<#propertysNav#>", classBuilderNavPropertys);

            return classBuilder;
        }
        protected string MakePropertyNavigationModels(TableInfo tableInfo, Context configContext, string TextTemplateNavPropertysCollection, string TextTemplateNavPropertysInstace, string classBuilder)
        {
            if (!configContext.MakeNavigationPropertys)
                return classBuilder
                .Replace("<#propertysNav#>", string.Empty);

            var classBuilderNavPropertys = string.Empty;
            var usingPropertysNav = string.Empty;

            foreach (var item in tableInfo.ReletedClasss)
            {
                var TextTemplateNavPropertys = item.NavigationType == NavigationType.Instance ? TextTemplateNavPropertysInstace : TextTemplateNavPropertysCollection;

                if (item.ClassName == tableInfo.ClassName)
                    classBuilderNavPropertys += String.Format("{0}{1}", TabModels(), TextTemplateNavPropertys
                        .Replace("<#className#>", item.ClassName)
                        .Replace("<#classNameNav#>", String.Format("{0}Self", item.ClassName)));

                if (item.ClassName != tableInfo.ClassName)
                    classBuilderNavPropertys += String.Format("{0}{1}", TabModels(), TextTemplateNavPropertys
                        .Replace("<#className#>", item.ClassName)
                        .Replace("<#classNameNav#>", item.ClassName));


                if (item.Module != configContext.Module)
                {
                    var usingReference = String.Format("using {0}.Domain;{1}", item.Namespace, System.Environment.NewLine);
                    if (!usingPropertysNav.Contains(usingReference))
                        usingPropertysNav += usingReference;
                }

            }

            classBuilder = classBuilder
                .Replace("<#propertysNav#>", classBuilderNavPropertys);

            return classBuilder;

        }

        private string MakeReletedIntanceValues(TableInfo tableInfo, Context configContext, string TextTemplateReletedValues, string classBuilder)
        {
            var classBuilderReletedValues = string.Empty;

            foreach (var item in tableInfo.ReletedClasss.Where(_ => _.NavigationType == NavigationType.Instance))
            {
                var itemvalue = TextTemplateReletedValues.
                       Replace("<#className#>", item.Table).
                       Replace("<#FKeyName#>", item.PropertyNameFk).
                       Replace("<#KeyName#>", item.PropertyNamePk);

                classBuilderReletedValues += string.Format("{0}{1}", itemvalue, System.Environment.NewLine);

            }

            classBuilder = classBuilder.Replace("<#reletedValues#>", classBuilderReletedValues);
            classBuilder = MakeReletedNamespace(tableInfo, configContext, classBuilder);

            return classBuilder;
        }

        private string MakeReletedNamespace(TableInfo tableInfo, Context configContext, string classBuilder)
        {
            var namespaceReletedApp = string.Empty;
            var namespaceReletedAppTest = string.Empty;
            var reletedClass = tableInfo.ReletedClasss;

            if (reletedClass != null)
            {
                foreach (var item in reletedClass.Where(_ => _.NavigationType == NavigationType.Instance))
                {

                    if (item.NamespaceApp != configContext.Namespace)
                        namespaceReletedApp = !string.IsNullOrEmpty(item.NamespaceApp) ? string.Format("using {0}.Application;", item.NamespaceApp) : string.Empty;

                    if (item.NamespaceApp != configContext.Namespace)
                        namespaceReletedAppTest = !string.IsNullOrEmpty(item.NamespaceApp) ? string.Format("using {0}.Application.Test;", item.NamespaceApp) : string.Empty;

                }
            }

            classBuilder = classBuilder.Replace("<#namespaceReleted#>", namespaceReletedApp);
            classBuilder = classBuilder.Replace("<#namespaceReletedTest#>", namespaceReletedAppTest);

            return classBuilder;
        }

        protected bool ExistsFields(IEnumerable<Info> infos, params  string[] fields)
        {
            var existsFields = false;

            foreach (var item in fields)
            {
                existsFields = infos.Where(_ => _.PropertyName.Equals(item)).Any();
                if (!existsFields)
                    return false;
            }

            return true;
        }

        private string MakeKey(IEnumerable<Info> infos, string textTemplateCompositeKey, string classBuilderitemplateCompositeKey)
        {
            var compositeKey = infos.Where(_ => _.IsKey == 1);
            if (compositeKey.Count() > 0)
            {
                var CompositeKeys = string.Empty;
                foreach (var item in compositeKey)
                    CompositeKeys += string.Format("d.{0},", item.PropertyName);


                var itemTemplateCompositeKey = textTemplateCompositeKey
                          .Replace("<#Keys#>", CompositeKeys);

                classBuilderitemplateCompositeKey = string.Format("{0}{1}", TabMaps(), itemTemplateCompositeKey);
            }
            return classBuilderitemplateCompositeKey;
        }

        protected void Dispose()
        {
            if (this.conn != null)
            {
                this.conn.Close();
                this.conn.Dispose();
            }
        }
    }
}
