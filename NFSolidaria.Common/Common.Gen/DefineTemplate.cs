using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    public static  class DefineTemplate
    {

        public static string DefineTemplateAppPartial(TableInfo tableInfo)
        {
            return tableInfo.InheritQuery ? "inherit\\app.partial.inherit" : "app.partial";
        }
        public static string DefineTemplateCustomFilters()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "inherit\\models.customfilters.inherit");
        }
        public static string DefineTemplateApiStart(TableInfo tableInfo)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", tableInfo.InheritQuery ? "inherit\\api.start.inherit" : "api.start");
        }
        public static string DefineTemplateApp(TableInfo tableInfo)
        {
            if (tableInfo.InheritQuery) return "inherit\\app.inherit";
            if (tableInfo.ModelBaseWithoutGets) return tableInfo.MakeCrud ? "withoutgets\\app.crud.withoutgets" : "withoutgets\\app.withoutgets";
            return tableInfo.MakeCrud ? "app.crud" : "app";
        }
        public static string DefineTemplateMaps(TableInfo tableInfo)
        {
            return tableInfo.InheritQuery ? "inherit\\maps.inherit" : "maps";
        }
        public static string DefineTemplateFiltersPartial(TableInfo tableInfo)
        {
            return tableInfo.InheritQuery ? "inherit\\Filter.partial.inherit" : "filter.partial";
        }

        public static string DefineTemplateModelPartial(TableInfo tableInfo)
        {
            if (tableInfo.InheritQuery)
                return tableInfo.MakeApp ? "inherit\\models.partial.Inherit" : "inherit\\models.partial.inherit.min";

            return tableInfo.MakeCrud ? "models.partial.crud" : "models.partial";

        }

        public static string DefineTemplateModels(TableInfo tableInfo)
        {

            if (tableInfo.ModelBase)
                return tableInfo.MakeCrud ? "base\\models.crud.base" : "base\\models.base";

            if (tableInfo.ModelBaseWithoutGets)
                return tableInfo.MakeCrud ? "withoutgets\\models.crud.base.withoutgets" : "withoutgets\\models.base.withoutgets";


            return tableInfo.MakeCrud ? "models.crud" : "models";
        }

        public static string DefineTemplateSimpleFilters(TableInfo tableInfo)
        {
            return "inherit\\models.simplefilters.inherit";
        }

        public static string DefineTemplateApi(TableInfo tableInfo)
        {
            if (tableInfo.InheritQuery) return "inherit\\api.inherit";
            if (tableInfo.ModelBaseWithoutGets) return tableInfo.MakeCrud ? "withoutgets\\api.crud.withoutgets" : "withoutgets\\api.withoutgets";
            return tableInfo.MakeCrud ? "api.crud" : "api";
        }

        public static string DefineTemplateMapPartialinherit(TableInfo tableInfo)
        {
            return tableInfo.InheritQuery ? "inherit\\maps.partial.inherit" : "maps.partial";
        }

        public static string DefineTemplateMapperProfile(TableInfo tableInfo)
        {
            return tableInfo.InheritQuery ? "inherit\\profile.inherit" : "profile";
        }
    }
}
