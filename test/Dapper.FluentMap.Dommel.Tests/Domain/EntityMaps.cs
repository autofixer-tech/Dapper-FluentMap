using System;
using Dapper.FluentMap.Configuration;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;

namespace Dapper.FluentMap.Dommel.Tests.Domain;

public class EntityMaps
{
    internal class EntityLongMap : ConventionalDommelEntityMap<Entity<long>>
    {
        public EntityLongMap()
        {
            Map(_ => _.Id).ToColumn(nameof(Entity<long>.Id).ToSnakeCase());
        }
    }

    internal class EntityStringMap : ConventionalDommelEntityMap<Entity<string>>
    {
        public EntityStringMap()
        {
            Map(_ => _.Id).ToColumn(nameof(Entity<string>.Id).ToSnakeCase());
        }
    }

    internal class EntityIntMap : ConventionalDommelEntityMap<Entity<int>>
    {
        public EntityIntMap()
        {
            Map(_ => _.Id).ToColumn(nameof(Entity<int>.Id).ToSnakeCase());
        }
    }

    internal class EntityGuidMap : ConventionalDommelEntityMap<Entity<Guid>>
    {
        public EntityGuidMap()
        {
            Map(_ => _.Id).ToColumn(nameof(Entity<Guid>.Id).ToSnakeCase());
        }
    }

    public static void Map(FluentMapConfiguration config)
    {
        config.AddMap(new EntityLongMap());
        config.AddMap(new EntityStringMap());
        config.AddMap(new EntityIntMap());
        config.AddMap(new EntityGuidMap());
    }
}