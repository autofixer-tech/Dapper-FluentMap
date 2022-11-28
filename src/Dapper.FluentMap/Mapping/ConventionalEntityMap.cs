using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Dapper.FluentMap.Utils;

namespace Dapper.FluentMap.Mapping
{
    /// <summary>
    /// Serves as the base class for all overriding entity mapping implementations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TPropertyMap">The type of the property mapping.</typeparam>
    public abstract class ConventionalEntityMapBase<TEntity, TPropertyMap> : IEntityMap<TEntity>
        where TPropertyMap : IPropertyMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMapBase{TEntity, TPropertyMap}"/> class.
        /// </summary>
        protected ConventionalEntityMapBase()
        {
            PropertyMaps = new List<IPropertyMap>();
        }

        /// <summary>
        /// Gets the collection of mapped properties.
        /// </summary>
        public IList<IPropertyMap> PropertyMaps { get; }

        /// <summary>
        /// Returns an instance of <typeparamref name="TPropertyMap"/> which can perform custom mapping
        /// for the specified property on <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="expression">Expression to the property on <typeparamref name="TEntity"/>.</param>
        /// <returns>The created <see cref="T:Dapper.FluentMap.Mapping.PropertyMap"/> instance. This enables a fluent API.</returns>
        protected TPropertyMap Map(Expression<Func<TEntity, object>> expression)
        {
            var info = (PropertyInfo)ReflectionHelper.GetMemberInfo(expression);
            var propertyMap = GetPropertyMap(info);
            return AddOrUpdatePropertyMap(propertyMap);
        }

        /// <summary>
        /// Returns an instance of IPropertyMap which can perform custom mapping
        /// </summary>
        /// <param name="propertyMap"></param>
        /// <returns>The created <see cref="T:Dapper.FluentMap.Mapping.IPropertyMap"/> instance. This enables a fluent API.</returns>
        protected IPropertyMap Map(IPropertyMap propertyMap)
        {
            return AddOrUpdatePropertyMap(propertyMap);
        }

        /// <summary>
        /// When overridden in a derived class, gets the property mapping for the specified property.
        /// </summary>
        /// <param name="info">The <see cref="PropertyInfo"/> for the property.</param>
        /// <returns>An instance of <typeparamref name="TPropertyMap"/>.</returns>
        protected abstract TPropertyMap GetPropertyMap(PropertyInfo info);

        private TInternalPropertyMap AddOrUpdatePropertyMap<TInternalPropertyMap>(TInternalPropertyMap propertyMap)
            where TInternalPropertyMap : IPropertyMap
        {
            var existingMap = PropertyMaps.SingleOrDefault(_ => _.PropertyInfo.Name == propertyMap.PropertyInfo.Name);
            if (existingMap != null)
            {
                PropertyMaps.Remove(existingMap);
            }

            PropertyMaps.Add(propertyMap);
            return propertyMap;
        }
    }

    /// <summary>
    /// Represents a typed mapping of an entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to configure the mapping for.</typeparam>
    public abstract class ConventionalEntityMap<TEntity> : ConventionalEntityMapBase<TEntity, PropertyMap>
        where TEntity : class
    {
        /// <inheritdoc />
        protected override PropertyMap GetPropertyMap(PropertyInfo info)
        {
            return new PropertyMap(info);
        }
    }
}