using System;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using Dommel;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <summary>
    /// Implements the <see cref="ITableNameResolver"/> interface by using the configured mapping.
    /// </summary>
    public class DommelTableNameResolver : ITableNameResolver
    {
        private static readonly ITableNameResolver DefaultResolver = new DefaultTableNameResolver();

        /// <inheritdoc />
        public string ResolveTableName(Type type)
        {
            if (FluentMapper.EntityMaps.TryGetValue(type, out var entityMap))
            {
                var mapping = entityMap as IDommelEntityMap;
                if (mapping?.TableName != null)
                {
                    return mapping.TableName;
                }
            }

            return DefaultResolver.ResolveTableName(type);
        }
    }
}