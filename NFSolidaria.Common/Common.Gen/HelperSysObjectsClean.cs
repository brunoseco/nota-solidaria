using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace Common.Gen
{

    public abstract class HelperSysObjectsClean : HelperSysObjectsBase
    {
        public HelperSysObjectsClean(IEnumerable<Context> contexts)
        {
            this.Contexts = contexts;
            PathOutput.UsePathProjects = true;
        }
        
        public void MakeClass(Context config)
        {
            MakeClass(config, string.Empty, true);
        }
        public void MakeClass(Context config, bool UsePathProjects)
        {
            MakeClass(config, string.Empty, UsePathProjects);
        }
        public void MakeClass(Context config, string RunOnlyThisClass, bool UsePathProjects)
        {
            PathOutput.UsePathProjects = UsePathProjects;
            
            TemplateByTableInfoFields(config, RunOnlyThisClass);
            TemplatesByTableleInfo(config);
            TemplateDbContextGenerateViews(config);

            this.Dispose();

        }

        private void TemplatesByTableleInfo(Context config)
        {
            foreach (var tableInfo in config.TableInfo)
            {
                ExecuteTemplateCustomFilters(tableInfo, config);
                ExecuteConfigDomain(tableInfo, config);
                ExecuteTemplateDbContext(tableInfo, config);
                ExecuteTemplateApiConfig(tableInfo, config);
                ExecuteTemplateMapperProfile(tableInfo, config);
                ExecuteTemplateContainer(tableInfo, config);
                ExecuteTemplateContainerPartial(tableInfo, config);
                ExecuteTemplateAutoMapper(tableInfo, config);
                //ExecuteTemplateUri(tableInfo, config);
                //ExecuteTemplateApiTestsBasic(tableInfo, config);
            }
        }

        private void TemplateByTableInfoFields(Context config, string RunOnlyThisClass)
        {
            var qtd = 0;
            foreach (var tableInfo in config.TableInfo)
            {

                qtd++;
                this.Open(config.ConnectionString);
                var commandText = new StringBuilder();

                commandText.Append("SELECT ");
                commandText.Append(" dbo.sysobjects.name AS Tabela,");
                commandText.Append(" dbo.syscolumns.name AS NomeColuna,");
                commandText.Append(" dbo.syscolumns.length AS Tamanho,");
                commandText.Append(" isnull(pk.is_primary_key,0) AS Chave,");
                commandText.Append(" dbo.syscolumns.isnullable AS Nulo,");
                commandText.Append(" dbo.systypes.name AS Tipo");
                commandText.Append(" FROM ");
                commandText.Append(" dbo.syscolumns INNER JOIN");
                commandText.Append(" dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id INNER JOIN");
                commandText.Append(" dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype ");
                commandText.Append(" LEFT JOIN (");
                commandText.Append(" Select Tablename, is_primary_key,ColumnName from (SELECT  i.name AS IndexName,");
                commandText.Append(" OBJECT_NAME(ic.OBJECT_ID) AS TableName,");
                commandText.Append(" COL_NAME(ic.OBJECT_ID,ic.column_id) AS ColumnName,");
                commandText.Append(" i.is_primary_key ");
                commandText.Append(" FROM sys.indexes AS i INNER JOIN ");
                commandText.Append(" sys.index_columns AS ic ON  i.OBJECT_ID = ic.OBJECT_ID");
                commandText.Append(" AND i.index_id = ic.index_id");
                commandText.Append(" WHERE   i.is_primary_key = 1) as TB_PRIMARYS) as pk");
                commandText.Append(" ON pk.tablename =  dbo.sysobjects.name and pk.ColumnName = dbo.syscolumns.name");
                commandText.Append(" WHERE ");
                commandText.Append(" (dbo.sysobjects.name = '" + tableInfo.TableName + "') ");
                commandText.Append(" AND ");
                commandText.Append(" (dbo.systypes.status <> 1) ");
                commandText.Append(" ORDER BY ");
                commandText.Append(" dbo.sysobjects.name, ");
                commandText.Append(" dbo.syscolumns.colorder ");

                var comando = new SqlCommand(commandText.ToString(), this.conn);
                var reader = comando.ExecuteReader();
                var infos = new UniqueListInfo();


                tableInfo.ClassName = MakeClassName(tableInfo);
                var reletedClass = new UniqueListInfo();
                reletedClass.AddRange(GetReletaedClasses(config, tableInfo.TableName));
                reletedClass.AddRange(GetReletaedIntancesComplementedClasses(config, tableInfo.TableName));
                tableInfo.ReletedClasss = reletedClass;


                while (reader.Read())
                {
                    infos.Add(new Info
                    {
                        Table = reader["Tabela"].ToString(),
                        ClassName = tableInfo.ClassName,
                        ColumnName = reader["NomeColuna"].ToString(),
                        PropertyName = MakePropertyName(reader["NomeColuna"].ToString(), tableInfo.ClassName, Convert.ToInt32(reader["Chave"])),
                        Length = reader["Tamanho"].ToString(),
                        IsKey = Convert.ToInt32(reader["Chave"]),
                        isNullable = Convert.ToInt32(reader["Nulo"]),
                        Type = TypeConvertCSharp(reader["tipo"].ToString(), Convert.ToInt32(reader["Nulo"])),
                        TypeOriginal = reader["tipo"].ToString(),
                        Module = config.Module,
                        Namespace = config.Namespace,

                    });


                }
                reader.Close();

                DefineInfoKey(tableInfo, infos);


                if (infos.Count == 0)
                {
                    if (config.DeleteFilesNotFoundTable)
                        DeleteFilesNotFoudTable(config, tableInfo);

                    if (config.AlertNotFoundTable)
                        throw new Exception("Tabela " + tableInfo.TableName + " Não foi econtrada");

                    continue;
                }

                if (!string.IsNullOrEmpty(RunOnlyThisClass))
                {
                    if (tableInfo.TableName != RunOnlyThisClass)
                        continue;
                }

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.CursorLeft = 10;
                Console.WriteLine(string.Format("{0} [{1}]", tableInfo.TableName, qtd));

                //ExecuteFixModels(tableInfo, config, infos);

                ExecuteTemplateModels(tableInfo, config, infos);
                ExecuteTemplateModelsPartial(tableInfo, config, infos);
                ExecuteTemplateSimpleFiltersInherit(tableInfo, config, infos);
                ExecuteTemplateModelsValiadation(tableInfo, config, infos);
                ExecuteTemplateModelsValiadationPartial(tableInfo, config, infos);
                ExecuteTemplateModelsCustom(tableInfo, config, infos);
                ExecuteTemplateFilter(tableInfo, config, infos);
                ExecuteTemplateFilterPartial(tableInfo, config, infos);
                ExecuteTemplateMaps(tableInfo, config, infos);
                ExecuteTemplateMapsPartial(tableInfo, config, infos);
                ExecuteTemplateDbContextInherit(tableInfo, config);
                ExecuteTemplateApp(tableInfo, config, infos);
                ExecuteTemplateAppPartial(tableInfo, config, infos);
                ExecuteTemplateDto(tableInfo, config, infos);
                ExecuteTemplateDtoSpecialized(tableInfo, config, infos);
                ExecuteTemplateDtoSpecializedResult(tableInfo, config, infos);
                ExecuteTemplateApi(tableInfo, config, infos);

            }
        }
        private static void MigatePathFileDomain(Context config, TableInfo tableInfo)
        {
            var fileBaseName = tableInfo.ClassName;
            var pathBase = Path.GetDirectoryName(PathOutput.PathOutputDomainModels(tableInfo, config)).Replace(fileBaseName, "");

            var FileVariants = new string[] { 
                    string.Format("{0}.cs", fileBaseName),
                    string.Format("{0}.ext.cs", fileBaseName),
                    string.Format("{0}.Validation.cs", fileBaseName),
                    string.Format("{0}.Validation.ext.cs", fileBaseName),
                    string.Format("{0}Custom.ext.cs", fileBaseName),
                };

            MigatePathFile(pathBase, fileBaseName, FileVariants);

        }
        private static void MigatePathFileDto(Context config, TableInfo tableInfo)
        {
            var fileBaseName = tableInfo.ClassName;
            var pathBase = Path.GetDirectoryName(PathOutput.PathOutputDto(tableInfo, config)).Replace(fileBaseName, "");

            var FileVariants = new string[] { 
                    string.Format("{0}Dto.cs", fileBaseName),
                    string.Format("{0}DtoSpecialized.ext.cs", fileBaseName),
                    string.Format("{0}DtoSpecializedResult.ext.cs", fileBaseName),
                };

            MigatePathFile(pathBase, fileBaseName, FileVariants);

        }
        private static void MigatePathFile(string pathBase, string fileBaseName, string[] FileVariants)
        {

            var folder = Path.Combine(pathBase, fileBaseName);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            foreach (var file in FileVariants)
            {
                var fileFound = new DirectoryInfo(pathBase).GetFiles(file).SingleOrDefault();
                if (fileFound != null)
                {
                    if (File.Exists(fileFound.FullName))
                    {
                        fileFound.CopyTo(Path.Combine(folder, file));
                        fileFound.Delete();
                    }
                }

            }
        }

        //TODO: Algum tipos de Arquivos não estão sendo Deletados
        private static void DeleteFilesNotFoudTable(Context config, TableInfo tableInfo)
        {
            var PathOutputMaps = PathOutput.PathOutputMaps(tableInfo, config);
            var PathOutputDomainModelsValidation = PathOutput.PathOutputDomainModelsValidation(tableInfo, config);
            var PathOutputDomainModels = PathOutput.PathOutputDomainModels(tableInfo, config);
            var PathOutputApp = PathOutput.PathOutputApp(tableInfo, config);
            var PathOutputUri = PathOutput.PathOutputUri(tableInfo, config);
            var PathOutputDto = PathOutput.PathOutputDto(tableInfo, config);
            var PathOutputApi = PathOutput.PathOutputApi(tableInfo, config);
            var PathOutputApplicationTest = PathOutput.PathOutputApplicationTest(tableInfo, config);
            var PathOutputApplicationTestMoq = PathOutput.PathOutputApplicationTestMoq(tableInfo, config);
            var PathOutputApiTest = PathOutput.PathOutputApiTest(tableInfo, config);

            File.Delete(PathOutputMaps);
            File.Delete(PathOutputDomainModelsValidation);
            File.Delete(PathOutputDomainModels);
            File.Delete(PathOutputApp);
            File.Delete(PathOutputUri);
            File.Delete(PathOutputDto);
            File.Delete(PathOutputApi);
            File.Delete(PathOutputApplicationTest);
            File.Delete(PathOutputApplicationTestMoq);
            File.Delete(PathOutputApiTest);

        }

        private void DefineInfoKey(TableInfo tableInfo, List<Info> infos)
        {
            var keys = infos.Where(_ => _.IsKey == 1);

            var Keys = new List<string>();
            var KeysTypes = new List<string>();

            foreach (var item in keys)
            {
                Keys.Add(item.PropertyName);
                KeysTypes.Add(item.Type);
            }

            tableInfo.Keys = Keys;
            tableInfo.KeysTypes = KeysTypes;

        }

        private static bool IsPrimaryKey(SqlDataReader reader)
        {
            return Convert.ToInt32(reader["Chave"]) == 1;
        }

        private string MakePropertyName(string column, string className)
        {
            return MakePropertyName(column, className, 0);
        }
        protected virtual string MakePropertyName(string column, string className, int key)
        {

            if (column.ToLower() == "id")
                return string.Format("{0}Id", className);


            if (column.ToString().ToLower().StartsWith("id"))
            {
                var keyname = column.ToString().Replace("Id", "");
                return string.Format("{0}Id", keyname);
            }

            return column;
        }

        private static string MakePropertyNameKeyAlias(string column, string className)
        {

            if (column.ToLower() == "id")
                return string.Format("{0}Id", className);


            return column;
        }

        protected virtual string MakeClassName(TableInfo tableInfo)
        {
            return MakeClassName(tableInfo.ClassName, tableInfo.TableName);
        }

        protected virtual string MakeClassName(string className, string tableName)
        {
            if (string.IsNullOrEmpty(className))
                return tableName;
            else
                return className;
        }

        public IEnumerable<Info> GetReletaedIntancesComplementedClasses(Context config, string tableName)
        {

            var commandText = new StringBuilder();


            commandText.Append("SELECT ");
            commandText.Append("KCU1.CONSTRAINT_NAME AS 'FK_Nome_Constraint' ");
            commandText.Append(",KCU1.TABLE_NAME AS 'FK_Nome_Tabela' ");
            commandText.Append(",KCU1.COLUMN_NAME AS 'FK_Nome_Coluna' ");
            commandText.Append(",FK.is_disabled AS 'FK_Esta_Desativada' ");
            commandText.Append(",KCU2.CONSTRAINT_NAME AS 'PK_Nome_Constraint_Referenciada' ");
            commandText.Append(",KCU2.TABLE_NAME AS 'PK_Nome_Tabela_Referenciada' ");
            commandText.Append(",KCU2.COLUMN_NAME AS 'PK_Nome_Coluna_Referenciada' ");
            commandText.Append("FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC ");
            commandText.Append("JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1 ");
            commandText.Append("ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG ");
            commandText.Append("AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA ");
            commandText.Append("AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME ");
            commandText.Append("JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU2 ");
            commandText.Append("ON KCU2.CONSTRAINT_CATALOG = RC.UNIQUE_CONSTRAINT_CATALOG  ");
            commandText.Append("AND KCU2.CONSTRAINT_SCHEMA = RC.UNIQUE_CONSTRAINT_SCHEMA ");
            commandText.Append("AND KCU2.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME ");
            commandText.Append("AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION ");
            commandText.Append("JOIN sys.foreign_keys FK on FK.name = KCU1.CONSTRAINT_NAME ");
            commandText.Append("Where ");
            commandText.Append(string.Format("KCU1.TABLE_NAME = '{0}' ", tableName));
            commandText.Append("Order by  ");
            commandText.Append("KCU1.TABLE_NAME ");

            var comando = new SqlCommand(commandText.ToString(), this.conn);
            var reader = comando.ExecuteReader();



            var reletedClasses = new UniqueListInfo();

            while (reader.Read())
            {
                var tableNamePk = reader["PK_Nome_Tabela_Referenciada"].ToString();
                var classNamePk = reader["PK_Nome_Tabela_Referenciada"].ToString();

                var tableNameFk = reader["PK_Nome_Tabela_Referenciada"].ToString();
                var classNameFk = reader["PK_Nome_Tabela_Referenciada"].ToString();


                var module = GetModuleFromContextByTableName(tableNamePk, config.Module);

                var _namespace = GetNameSpaceFromContextByTableName(tableNamePk, config.Module);

                var namespaceApp = GetNameSpaceFromContextWithMakeAppByTableName(tableNamePk, config.Module);

                var namespaceDto = GetNameSpaceFromContextWithMakeDtoByTableName(tableNamePk, config.Module);


                if (AppNotExpose(namespaceApp))
                    continue;

                reletedClasses.Add(new Info
                {
                    Table = tableNamePk,
                    ClassName = MakeClassName(classNamePk, tableNamePk),
                    Module = module,
                    Namespace = _namespace,
                    NamespaceApp = namespaceApp,
                    NamespaceDto = namespaceDto,
                    PropertyNamePk = MakePropertyName(reader["PK_Nome_Coluna_Referenciada"].ToString(), MakeClassName(classNamePk, tableNamePk)),
                    PropertyNameFk = MakePropertyName(reader["FK_Nome_Coluna"].ToString(), tableName),
                    NavigationType = NavigationType.Instance
                });
            }



            comando.Dispose();
            reader.Close();


            return reletedClasses;

        }

        private string GetNameSpaceFromContextWithMakeDtoByTableName(string tableName, string module)
        {
            var namespaceDto = this.Contexts
               .Where(_ => _.Module == module)
               .Where(_ => _.TableInfo
                   .Where(__ => __.TableName.Equals(tableName))
                   .Where(___ => ___.MakeDto)
                   .Any())
               .Select(_ => _.Namespace).FirstOrDefault();
            return namespaceDto;
        }

        private string GetNameSpaceFromContextWithMakeAppByTableName(string tableName, string module)
        {
            var namespaceApp = this.Contexts
                .Where(_ => _.Module == module)
                .Where(_ => _.TableInfo
                    .Where(__ => __.TableName.Equals(tableName))
                    .Where(___ => ___.MakeApp)
                    .Any())
                .Select(_ => _.Namespace).FirstOrDefault();
            return namespaceApp;
        }

        private string GetNameSpaceFromContextByTableName(string tableName, string module)
        {
            var _namespace = this.Contexts
                .Where(_ => _.Module == module)
                .Where(_ => _.TableInfo
                    .Where(__ => __.TableName.Equals(tableName)).Any())
                .Select(_ => _.Namespace).FirstOrDefault();
            return _namespace;
        }

        private string GetModuleFromContextByTableName(string tableName, string module)
        {
            var result = this.Contexts
                .Where(_ => _.Module == module)
                .Where(_ => _.TableInfo
                    .Where(__ => __.TableName.Equals(tableName)).Any())
                .Select(_ => _.Module).FirstOrDefault();
            return result;
        }

        public IEnumerable<Info> GetReletaedClasses(Context config, string tablelName)
        {

            var commandText = new StringBuilder().Append(string.Format("EXEC sp_fkeys '{0}'", tablelName));

            var comando = new SqlCommand(commandText.ToString(), this.conn);
            var reader = comando.ExecuteReader();


            var reletedClasses = new UniqueListInfo();
            while (reader.Read())
            {
                var tableName = reader["FKTABLE_NAME"].ToString();
                var className = reader["FKTABLE_NAME"].ToString();


                var module = GetModuleFromContextByTableName(tableName, config.Module);

                var _namespace = GetNameSpaceFromContextByTableName(tableName, config.Module);

                var namespaceApp = GetNameSpaceFromContextWithMakeAppByTableName(tableName, config.Module);

                var namespaceDto = GetNameSpaceFromContextWithMakeDtoByTableName(tableName, config.Module);


                if (AppNotExpose(namespaceApp))
                    continue;

                reletedClasses.Add(new Info
                {
                    Table = tableName,
                    ClassName = MakeClassName(className, tableName),
                    Module = module,
                    Namespace = _namespace,
                    NamespaceApp = namespaceApp,
                    NamespaceDto = namespaceDto,
                    PropertyNameFk = MakePropertyName(reader["FKCOLUMN_NAME"].ToString(), MakeClassName(className, tableName)),
                    PropertyNamePk = MakePropertyName(reader["PKCOLUMN_NAME"].ToString(), MakeClassName(className, tableName)),
                    NavigationType = reader["FKCOLUMN_NAME"].ToString().Equals("Id") ? NavigationType.Instance : NavigationType.Collettion
                });

            }



            comando.Dispose();
            reader.Close();



            return reletedClasses;

        }

        private bool IsOneToOneRelation(SqlDataReader reader)
        {
            return reader["FKCOLUMN_NAME"].ToString() == reader["PKCOLUMN_NAME"].ToString();
        }

        private bool AppNotExpose(string namespaceApp)
        {
            return string.IsNullOrEmpty(namespaceApp);
        }


        private void ClearContextCodeFiles(Context config, string path, string subfolder, Func<TableInfo, bool> predicate, string noDeleteFilesPartner = null)
        {
            if (!config.ClearAllFiles)
                return;

            if (String.IsNullOrEmpty(path))
                return;

            var directory = new DirectoryInfo(Path.Combine(path, subfolder));
            if (!directory.Exists)
                return;

            var files = directory.GetFiles().Where(_ => _.Extension == ".cs");
            foreach (var item in files)
            {

                if (!String.IsNullOrEmpty(noDeleteFilesPartner))
                    if (item.Name.Contains(noDeleteFilesPartner))
                        continue;


                if (item.Name.Contains(".ext"))
                    continue;

                var deleteDenied = this.Contexts
                    .Where(_ => _.TableInfo
                        .Where(__ => item.Name.Contains(__.TableName))
                        .Where(predicate).Any()).Any();


                if (deleteDenied)
                    continue;


                item.Delete();
            }

        }


        protected string makeClassName(string tableName)
        {
            return MakeClassNameTemplateDefault(tableName);
        }


        protected override bool ExistsAuditFields(IEnumerable<Info> infos)
        {
            return ExistsFields(infos, auditFields);
        }

        protected override string makeAuditRow(TableInfo tableInfo, bool generateAudit, Info item, string textTemplateAudit)
        {
            if (generateAudit)
                return MakeAuditFields(tableInfo, item, textTemplateAudit);

            return string.Empty;
        }

        private string MakeAuditFields(TableInfo tableInfo, Info item, string textTemplateAudit)
        {
            var pathTemplateAudit = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.audit");

            if (string.IsNullOrEmpty(textTemplateAudit))
                textTemplateAudit = File.ReadAllText(pathTemplateAudit);

            textTemplateAudit = textTemplateAudit.Replace("<#className#>", tableInfo.ClassName);

            if (item.PropertyName == "UserCreateId" || item.PropertyName == "UserAlterId")
            {

                var cast = String.Format("({0})userId", TypeConvertCSharp(item.TypeOriginal, item.isNullable));

                if (item.PropertyName == "UserCreateId")
                    textTemplateAudit = textTemplateAudit.Replace("<#propertCastInsert#>", cast);

                if (item.PropertyName == "UserAlterId")
                    textTemplateAudit = textTemplateAudit.Replace("<#propertCastUpdate#>", cast);

            }
            return textTemplateAudit;
        }

        private string MakeAuditLog(Info item)
        {
            var pathTemplateAudit = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.audit.log");
            var textTemplateAudit = File.ReadAllText(pathTemplateAudit);
            return textTemplateAudit;
        }

        protected override bool IsAuditField(string field)
        {
            return auditFields.Contains(field);
        }
    }
}
