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
    public class TablePrefix
    {
        public string Prefix { get; set; }
        public string ClassName { get; set; }

    }
    public class HelperSysObjectsWithPrefix : HelperSysObjectsBase
    {
        private List<TablePrefix> TablePrefixes;

        public HelperSysObjectsWithPrefix()
        {
            this.TablePrefixes = new List<TablePrefix>();
        }

        public void MakeClass(Context config)
        {


            foreach (var tableInfo in config.TableInfo)
            {
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
                var infos = new List<Info>();


                tableInfo.ClassName = string.IsNullOrEmpty(tableInfo.ClassName) ? makeClassName(tableInfo.TableName) : tableInfo.ClassName;

                var prefix = makePrefixTable(tableInfo);
                if (!string.IsNullOrEmpty(prefix))
                {
                    TablePrefixes.Add(new TablePrefix { ClassName = tableInfo.ClassName, Prefix = prefix });
                }

                while (reader.Read())
                {
                    infos.Add(new Info
                    {
                        Table = reader["Tabela"].ToString(),
                        ColumnName = reader["NomeColuna"].ToString(),
                        PropertyName = MakePropertyName(tableInfo, reader["NomeColuna"].ToString(), Convert.ToInt32(reader["Chave"])),
                        Length = reader["Tamanho"].ToString(),
                        IsKey = Convert.ToInt32(reader["Chave"]),
                        isNullable = Convert.ToInt32(reader["Nulo"]),
                        Type = TypeConvertCSharp(reader["tipo"].ToString(), Convert.ToInt32(reader["Nulo"])),
                        TypeOriginal = reader["tipo"].ToString(),
                    });

                }

                if (infos.Count > 0)
                {
                    ExecuteTemplateModels(tableInfo, config, infos);
                    ExecuteTemplateModelsPartial(tableInfo, config, infos);
                    ExecuteTemplateModelsValiadation(tableInfo, config, infos);
                    ExecuteTemplateMaps(tableInfo, config, infos);
                    ExecuteTemplateMapsPartial(tableInfo, config, infos);
                }

                this.Dispose();

            }

        }

        protected override bool ExistsAuditFields(IEnumerable<Info> infos)
        {
            return ExistsFields(infos, "UsuarioInclusaoId", "UsuarioAlteracaoId", "AlteracaoData", "InclusaoData");
        }

        protected override string makeAuditRow(TableInfo tableInfo,bool generateAudit, Info item, string textTemplateAudit)
        {
            var pathTemplateAudit = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "models.audit");
            textTemplateAudit = File.ReadAllText(pathTemplateAudit);

            if (item.IsKey == 1)
            {
                var expressionInclusion = item.Type == "string" ? string.Format("string.IsNullOrEmpty(this.{0})",
                    item.PropertyName) : string.Format("this.{0} == 0", item.PropertyName);

                textTemplateAudit = textTemplateAudit.Replace("<#ExpressionInclusion#>", expressionInclusion);
            }

            if (generateAudit && (item.PropertyName == "UsuarioInclusaoId" || item.PropertyName == "UsuarioAlteracaoId"))
            {

                var cast = String.Format("({0})userId", TypeConvertCSharp(item.TypeOriginal, item.isNullable));
                if (item.Type == "string")
                    cast = "userId.ToString()";

                if (item.PropertyName == "UsuarioInclusaoId")
                    textTemplateAudit = textTemplateAudit.Replace("<#propertCastInsert#>", cast);

                if (item.PropertyName == "UsuarioAlteracaoId")
                    textTemplateAudit = textTemplateAudit.Replace("<#propertCastUpdate#>", cast);

            }
            return textTemplateAudit;
        }

        public void addPrefix(TablePrefix prefix)
        {
            this.TablePrefixes.Add(prefix);
        }

        private string PrefixReplace(string columnName)
        {
            var parts = columnName.Split('_');

            foreach (var item in parts)
            {
                var newItem = TablePrefixes.Where(_ => _.Prefix == item).Select(_ => _.ClassName).SingleOrDefault();

                if (!string.IsNullOrEmpty(newItem))
                    columnName = columnName.Replace(item, newItem);
            }
            return columnName;
        }


        private string MakeClassNameTemplateX(string tableName)
        {

            var result = tableName.Substring(2);
            result = PrefixReplace(result);
            result = CamelCaseTransform(result);
            result = ClearEnd(result);
            return result;
        }

        public string MakePropertyNamePatnerX(string columnName)
        {
            var newColumnName = columnName;

            newColumnName = PrefixReplace(newColumnName);
            newColumnName = TranslateNames(newColumnName);
            newColumnName = TranslateNamesPatnerX(newColumnName);
            newColumnName = CamelCaseTransform(newColumnName);
            newColumnName = ClearEnd(newColumnName);

            return newColumnName;

        }

        private string MakePropertyName(TableInfo tableConfig, string columnName, int isPrimaryKey)
        {
            if (isPrimaryKey == 1)
            {
                if (tableConfig.TableName.Contains("_X_"))
                {
                    if (columnName.Contains("_X_"))
                        return MakePropertyNameForId(tableConfig);
                }
                else
                {
                    if (makePrefixTable(tableConfig) == makePrefixField(columnName))
                        return MakePropertyNameForId(tableConfig);
                }
            }

            if (columnName.Contains("_X_"))
                return MakePropertyNamePatnerX(columnName);
            else
            {
                return MakePropertyNameDefault(columnName);
            }
        }


        private string makeClassName(string tableName)
        {
            if (tableName.Contains("_X_"))
                return MakeClassNameTemplateX(tableName);
            else
                return MakeClassNameTemplateDefault(tableName);
        }


        protected override bool IsAuditField(string field)
        {
            throw new NotImplementedException();
        }
    }
}
