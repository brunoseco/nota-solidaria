using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    static class PathOutput
    {
        public static bool UsePathProjects { get; set; }

        private static string PathBase(string pathProject)
        {
            var pathOutputLocal = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");

            if (UsePathProjects)
                return String.IsNullOrEmpty(pathProject) ? pathOutputLocal : pathProject;

            return pathOutputLocal;
        }
    

        public static string PathOutputMapsPartial(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.InheritQuery)
                return PathOutputMapsPartialinherit(tableInfo, configContext);

            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassInfra);
            pathOutput = Path.Combine(pathBase, "Maps", string.Format("{0}Map.ext.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory("Maps", pathBase);
            return pathOutput;
        }

        public static string PathOutputMapsPartialinherit(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassInfra);
            var fileName = tableInfo.ClassName;
            pathOutput = Path.Combine(pathBase, "Maps", tableInfo.BoundedContext, fileName, string.Format("{0}Map.ext.{1}", fileName, "cs"));
            MakeDirectory(pathBase, "Maps", tableInfo.BoundedContext, fileName);
            return pathOutput;


        }

        public static string PathOutputMaps(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.InheritQuery)
                return PathOutputMapsInherit(tableInfo, configContext);

            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassInfra);
            pathOutput = Path.Combine(pathBase, "Maps", string.Format("{0}Map.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory("Maps", pathBase);
            return pathOutput;
        }

        public static string PathOutputMapsInherit(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassInfra);
            pathOutput = Path.Combine(pathBase, "Maps", tableInfo.BoundedContext, tableInfo.ClassName, string.Format("{0}Map.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(pathBase, "Maps", tableInfo.BoundedContext, tableInfo.ClassName);
            return pathOutput;
        }

        public static string PathOutputContextsInherit(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassInfra);
            var fileName = tableInfo.BoundedContext;
            pathOutput = Path.Combine(pathBase, "Maps", tableInfo.BoundedContext, string.Format("DbContext{0}.{1}", fileName, "cs"));
            MakeDirectory(pathBase, "Maps", tableInfo.BoundedContext);

            return pathOutput;
        }

        public static string PathOutputDbContext(Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassInfra);

            pathOutput = Path.Combine(pathBase, "Context", string.Format("DbContext{0}.{1}", configContext.Module, "cs"));
            MakeDirectory("Context", pathBase);

            return pathOutput;
        }

        public static string PathOutputDomainModelsPartial(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.InheritQuery)
                return PathOutputDomainModelsPartialinherit(tableInfo, configContext);

            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassDomain);

            var filename = tableInfo.ClassName;
            pathOutput = Path.Combine(pathBase, "Models", filename, string.Format("{0}.ext.{1}", filename, "cs"));
            MakeDirectory(pathBase, "Models", filename);

            return pathOutput;
        }

        public static string PathOutputDomainModelsPartialinherit(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassDomain);

            var filename = tableInfo.ClassName;
            pathOutput = Path.Combine(pathBase, "Models", tableInfo.BoundedContext, "Entitys", filename, string.Format("{0}.ext.{1}", filename, "cs"));
            MakeDirectory(pathBase, "Models", tableInfo.BoundedContext, "Entitys", filename);

            return pathOutput;
        }


        public static string PathOutputDomainModelsValidation(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassDomain);

            pathOutput = Path.Combine(pathBase, "Models", tableInfo.ClassName, string.Format("{0}.Validation.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(pathBase, "Models", tableInfo.ClassName);

            return pathOutput;

        }

        public static string PathOutputDomainModelsValidationPartial(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassDomain);

            pathOutput = Path.Combine(pathBase, "Models", tableInfo.ClassName, string.Format("{0}.Validation.ext.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(pathBase, "Models", tableInfo.ClassName);

            return pathOutput;

        }

        public static string PathOutputDomainModelsCustom(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.InheritQuery)
                return PathOutputDomainModelsCustomInherit(tableInfo, configContext);

            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassDomain);
            var filename = tableInfo.ClassName;
            pathOutput = Path.Combine(pathBase, "Models", filename, string.Format("{0}Custom.ext.{1}", filename, "cs"));
            MakeDirectory(pathBase, "Models", filename);

            return pathOutput;

        }

        public static string PathOutputDomainModelsCustomInherit(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassDomain);
            var filename = tableInfo.ClassName;
            pathOutput = Path.Combine(pathBase, "Models", tableInfo.BoundedContext, filename, string.Format("{0}Custom.ext.{1}", filename, "cs"));
            MakeDirectory(pathBase, "Models", tableInfo.BoundedContext, filename);

            return pathOutput;

        }

        public static string PathOutputCustomFilters(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassDomain);

            var filename = string.Format("{0}CustomFiltersFiltersExtensions.", tableInfo.ClassName);
            pathOutput = Path.Combine(pathBase, "Models", tableInfo.BoundedContext, "Entitys", tableInfo.ClassName, string.Format("{0}.ext.{1}", filename, "cs"));
            MakeDirectory(pathBase, "Models", tableInfo.BoundedContext, "Entitys", tableInfo.ClassName);

            return pathOutput;
        }
        public static string PathOutputSimpleFilters(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassDomain);

            var filename = string.Format("{0}SimpleFiltersFiltersExtensions.", tableInfo.ClassName);
            pathOutput = Path.Combine(pathBase, "Models", tableInfo.BoundedContext, "Entitys", tableInfo.ClassName, string.Format("{0}.ext.{1}", filename, "cs"));
            MakeDirectory(pathBase, "Models", tableInfo.BoundedContext, "Entitys", tableInfo.ClassName);

            return pathOutput;
        }

        public static string PathOutputDomainModels(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassDomain);
            pathOutput = Path.Combine(pathBase, "Models", tableInfo.ClassName, string.Format("{0}.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(pathBase, "Models", tableInfo.ClassName);


            return pathOutput;
        }

        public static string PathOutputApp(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.InheritQuery)
                return PathOutputAppInherit(tableInfo, configContext);

            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassApp);
            pathOutput = Path.Combine(pathBase, "Application", string.Format("{0}App.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory("Application", pathBase);
            return pathOutput;
        }

        public static string PathOutputAppInherit(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassApp);
            pathOutput = Path.Combine(pathBase, "Application", tableInfo.BoundedContext, string.Format("{0}App.{1}", tableInfo.InheritClassName, "cs"));
            MakeDirectory(pathBase, "Application", tableInfo.BoundedContext);
            return pathOutput;
        }

        public static string PathOutputAppPartial(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.InheritQuery)
                return PathOutputAppPartialInherit(tableInfo, configContext);

            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassApp);
            pathOutput = Path.Combine(pathBase, "Application", string.Format("{0}App.ext.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory("Application", pathBase);
            return pathOutput;
        }

        public static string PathOutputAppPartialInherit(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassApp);
            pathOutput = Path.Combine(pathBase, "Application", tableInfo.BoundedContext, string.Format("{0}App.ext.{1}", tableInfo.InheritClassName, "cs"));
            MakeDirectory(pathBase, "Application", tableInfo.BoundedContext);
            return pathOutput;
        }

        public static string PathOutputContainer(Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassApp);

            pathOutput = Path.Combine(pathBase, "Config", string.Format("ConfigContainer{0}.{1}", configContext.Module, "cs"));
            MakeDirectory("Config", pathBase);

            return pathOutput;
        }

        public static string PathOutputContainerPartial(Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassApp);

            pathOutput = Path.Combine(pathBase, "Config", string.Format("ConfigContainer{0}.ext.{1}", configContext.Module, "cs"));
            MakeDirectory("Config", pathBase);

            return pathOutput;
        }

        public static string PathOutputAutoMapper(Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassApp);

            pathOutput = Path.Combine(pathBase, "Config", string.Format("AutoMapperConfig{0}.{1}", configContext.Module, "cs"));
            MakeDirectory("Config", pathBase);


            return pathOutput;
        }

        public static string PathOutputAutoMapperProfile(Context configContext, TableInfo tableInfo)
        {
            if (tableInfo.InheritQuery)
                return PathOutputAutoMapperProfileInherit(configContext, tableInfo);

            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassApp);
            pathOutput = Path.Combine(pathBase, "Config", string.Format("DominioToDtoProfile{0}.{1}", configContext.Module, "cs"));
            MakeDirectory("Config", pathBase);
            return pathOutput;
        }

        public static string PathOutputAutoMapperProfileInherit(Context configContext, TableInfo tableInfo)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassApp);
            pathOutput = Path.Combine(pathBase, "Config", string.Format("DominioToDtoProfile{0}.{1}", tableInfo.BoundedContext, "cs"));
            MakeDirectory("Config", pathBase);
            return pathOutput;
        }

        public static string PathOutputWebApiConfig(Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassApi);

            pathOutput = Path.Combine(pathBase, "App_Start", string.Format("WebApiConfig.cs"));
            MakeDirectory("App_Start", pathBase);


            return pathOutput;
        }

        public static string PathOutputConfigDomain(Context configContext)
        {
            var pathOutput = string.Empty;


            var pathBase = PathBase(configContext.OutputClassDomain);

            pathOutput = Path.Combine(pathBase, "Base", string.Format("ConfigDomain{0}.{1}", configContext.Module, "cs"));
            MakeDirectory("Base", pathBase);

            return pathOutput;
        }



        public static string PathOutputUri(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.InheritQuery)
                return PathOutputUriInherit(tableInfo, configContext);

            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassUri);
            pathOutput = Path.Combine(pathBase, "Uri", configContext.Module, string.Format("{0}Uri.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(string.Format("Uri/{0}", configContext.Module), pathBase);
            return pathOutput;
        }

        public static string PathOutputUriInherit(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassUri);
            pathOutput = Path.Combine(pathBase, "Uri", configContext.Module, string.Format("{0}Uri.{1}", tableInfo.InheritClassName, "cs"));
            MakeDirectory(string.Format("Uri/{0}", configContext.Module), pathBase);
            return pathOutput;
        }

        public static string PathOutputPreCompiledView(Context configContext)
        {
            var pathOutput = string.Empty;


            var pathBase = PathBase(configContext.OutputClassInfra);

            pathOutput = Path.Combine(pathBase, "Context", string.Format("MappingViewCache{0}Genereted.{1}", configContext.Module, "cs"));
            MakeDirectory("Context", pathBase);

            return pathOutput;
        }

        public static string PathOutputFilter(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassFilter);
            pathOutput = Path.Combine(pathBase, "Filters", string.Format("{0}Filter.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory("Filters", pathBase);
            return pathOutput;
        }

        public static string PathOutputFilterPartial(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassFilter);
            var fileName = tableInfo.ClassName;
            pathOutput = Path.Combine(pathBase, "Filters", string.Format("{0}Filter.ext.{1}", fileName, "cs"));
            MakeDirectory("Filters", pathBase);
            return pathOutput;
        }

        public static string PathOutputDto(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.InheritQuery)
                return PathOutputDtoInherit(tableInfo, configContext);

            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassDto);
            pathOutput = Path.Combine(pathBase, "Dto", tableInfo.ClassName, string.Format("{0}Dto.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(pathBase, "Dto", tableInfo.ClassName);
            return pathOutput;
        }

        public static string PathOutputSummary(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassSummary);
            var fileName = tableInfo.ClassName;
            pathOutput = Path.Combine(pathBase, "Summary", string.Format("{0}Summary.{1}", fileName, "cs"));
            MakeDirectory("Summary", pathBase);
            return pathOutput;
        }

        public static string PathOutputDtoInherit(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassDto);
            pathOutput = Path.Combine(pathBase, "Dto", tableInfo.BoundedContext, tableInfo.ClassName, string.Format("{0}Dto.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(pathBase, "Dto", tableInfo.BoundedContext, tableInfo.ClassName);
            return pathOutput;
        }

        public static string PathOutputDtoSpecialized(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.InheritQuery)
                return PathOutputDtoSpecializedInherit(tableInfo, configContext);

            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassDto);
            pathOutput = Path.Combine(pathBase, "Dto", tableInfo.ClassName, string.Format("{0}DtoSpecialized.ext.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(pathBase, "Dto", tableInfo.ClassName);
            return pathOutput;
        }

        public static string PathOutputDtoSpecializedInherit(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassDto);
            pathOutput = Path.Combine(pathBase, "Dto", tableInfo.BoundedContext, tableInfo.ClassName, string.Format("{0}DtoSpecialized.ext.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(pathBase, "Dto", tableInfo.BoundedContext, tableInfo.ClassName);
            return pathOutput;
        }

        public static string PathOutputDtoSpecializedResult(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.InheritQuery)
                return PathOutputDtoSpecializedResultInherit(tableInfo, configContext);

            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassDto);
            pathOutput = Path.Combine(pathBase, "Dto", tableInfo.ClassName, string.Format("{0}DtoSpecializedResult.ext.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(pathBase, "Dto", tableInfo.ClassName);
            return pathOutput;
        }

        public static string PathOutputDtoSpecializedResultInherit(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassDto);
            pathOutput = Path.Combine(pathBase, "Dto", tableInfo.BoundedContext, tableInfo.ClassName, string.Format("{0}DtoSpecializedResult.ext.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(pathBase, "Dto", tableInfo.BoundedContext, tableInfo.ClassName);
            return pathOutput;
        }

        public static string PathOutputApi(TableInfo tableInfo, Context configContext)
        {
            if (tableInfo.InheritQuery)
                return PathOutputApiInherit(tableInfo, configContext);

            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassApi);
            pathOutput = Path.Combine(pathBase, "Controllers", string.Format("{0}Controller.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory("Controllers", pathBase);
            return pathOutput;
        }

        public static string PathOutputApiInherit(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var pathBase = PathBase(configContext.OutputClassApi);
            pathOutput = Path.Combine(pathBase, "Controllers", tableInfo.BoundedContext, string.Format("{0}Controller.{1}", tableInfo.InheritClassName, "cs"));
            MakeDirectory(pathBase, "Controllers", tableInfo.BoundedContext);
            return pathOutput;
        }

        public static string PathOutputApplicationTest(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassTestsApp);

            pathOutput = Path.Combine(pathBase, configContext.Module, string.Format("UnitTest{0}App.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(configContext.Module, pathBase);


            return pathOutput;
        }

        public static string PathOutputApplicationTestMoqPartial(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassTestsApp);

            pathOutput = Path.Combine(pathBase, "Moq", configContext.Module, string.Format("Helper{0}Moq.ext.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(Path.Combine("Moq", configContext.Module), pathBase);


            return pathOutput;
        }

        public static string PathOutputApplicationTestMoq(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassTestsApp);

            pathOutput = Path.Combine(pathBase, "Moq", configContext.Module, string.Format("Helper{0}Moq.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(Path.Combine("Moq", configContext.Module), pathBase);


            return pathOutput;
        }

        public static string PathOutputApiTest(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassTestsApi);

            pathOutput = Path.Combine(pathBase, configContext.Module, string.Format("UnitTest{0}Api.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(configContext.Module, pathBase);



            return pathOutput;
        }

        public static string PathOutputApiTestPartial(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;

            var pathBase = PathBase(configContext.OutputClassTestsApi);

            pathOutput = Path.Combine(pathBase, configContext.Module, string.Format("UnitTest{0}Api.ext.{1}", tableInfo.ClassName, "cs"));
            MakeDirectory(configContext.Module, pathBase);


            return pathOutput;
        }
        public static void MakeDirectory(string pathBase, params string[] paths)
        {
            MakeDirectory(Path.Combine(paths), pathBase);
        }
        public static void MakeDirectory(string directoryName, string pathBase)
        {

            var pathToGenerate = Path.Combine(pathBase, directoryName);

            if (!Directory.Exists(pathToGenerate))
                Directory.CreateDirectory(pathToGenerate);

        }



    }
}
