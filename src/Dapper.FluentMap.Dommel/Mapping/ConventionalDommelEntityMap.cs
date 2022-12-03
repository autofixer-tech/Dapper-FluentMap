using System;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.Dommel.Mapping
{
    /// <summary>
    /// Represents the typed mapping of an entity for Dommel.
    /// </summary>
    /// <typeparam name="TEntity">The type of an entity.</typeparam>
    public abstract class ConventionalDommelEntityMap<TEntity> : ConventionalEntityMapBase<TEntity, DommelPropertyMap>, IDommelEntityMap
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionalDommelEntityMap{TEntity}"/> class
        /// </summary>
        protected ConventionalDommelEntityMap()
        {
            var entityType = typeof(TEntity);
            Initialize(entityType);
        }

        private void Initialize(Type entityType)
        {
            if (!FluentMapper.TypeConventions.TryGetValue(entityType, out var conventions))
            {
                return;
            }

            var convention = conventions.First();
            foreach (var propertyMap in convention.PropertyMaps.Where(m => m.PropertyInfo.ReflectedType == entityType))
            {
                Map(propertyMap);
            }
        }

        /// <summary>
        /// Gets the <see cref="IPropertyMap"/> implementation for the current entity map.
        /// </summary>
        /// <param name="info">The information about the property.</param>
        /// <returns>An implementation of <see cref="Dapper.FluentMap.Mapping.IPropertyMap"/>.</returns>
        protected override DommelPropertyMap GetPropertyMap(PropertyInfo info)
        {
            return new DommelPropertyMap(info);
        }

        /// <summary>
        /// Gets the table name for this entity map.
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Sets the table name for the current entity.
        /// </summary>
        /// <param name="tableName">The name of the table in the database.</param>
        protected void ToTable(string tableName)
        {
            TableName = tableName;
        }

        /// <summary>
        /// Sets the table name for the current entity.
        /// </summary>
        /// <param name="tableName">The name of the table in the database.</param>
        /// <param name="schemaName">The name of the table schema in the database.</param>
        protected void ToTable(string tableName, string schemaName)
        {
            TableName = $"{schemaName}.{tableName}";
        }
    }
}