using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Infrastructure.ORM
{
    class MapperEstensions
    {
        public class ConfigMapper
        {

            public string tableName { get; set; }
            public IDictionary<string, string> CollumsMappers { get; set; }

        }
        public static ConfigMapper Config(Type type, DbContext context)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var entityTypes = metadata.GetItems<EntityType>(DataSpace.OSpace);

            var entityType = entityTypes.Single(e => objectItemCollection.GetClrType(e) == type.GetTypeInCollection());

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                    .Single()
                    .EntitySetMappings
                    .Single(s => s.EntitySet == entitySet);

            // Find the storage entity set (table) that the entity is mapped
            var tableEntitySet = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .StoreEntitySet;

            // Return the table name from the storage entity set
            var tableName = tableEntitySet.MetadataProperties["Table"].Value.ToString() ?? tableEntitySet.Name;

            // Find the storage property (column) that the property is mapped
            var columnNames = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .PropertyMappings
                .OfType<ScalarPropertyMapping>();

            var configMapper = new ConfigMapper();
            var collumsMappers = new Dictionary<string, string>();

            foreach (var item in columnNames)
            {
                collumsMappers.Add(item.Property.Name, item.Column.Name);
            }

            return new ConfigMapper
            {
                tableName = tableName,
                CollumsMappers = collumsMappers,
            };
        }


    }
}
