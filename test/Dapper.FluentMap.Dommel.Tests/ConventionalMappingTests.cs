using Dapper.FluentMap.Dommel.Mapping;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Dapper.FluentMap.Utils;
using Dommel;
using Xunit;

namespace Dapper.FluentMap.Dommel.Tests
{
    public class ConventionalMappingTests
    {
        public ConventionalMappingTests()
        {
            PreTest();
        }

        [Fact]
        public void EntityMapsCustomIdAsKey()
        {
            FluentMapper.Initialize(c => c.AddMap(new MapWithCustomIdPropertyMap()));

            var type = typeof(CustomIdEntity);
            var keyResolver = new Resolvers.DommelKeyPropertyResolver();
            var columnResolver = new Resolvers.DommelColumnNameResolver();

            var keys = keyResolver.ResolveKeyProperties(type);
            var columnName = columnResolver.ResolveColumnName(keys.Single().Property);
            
            Assert.Single(keys);
            Assert.Equal("customid", columnName);
        }

        [Fact]
        public void EntityMapsToSingleCustomId()
        {
            FluentMapper.Initialize(c => c.AddMap(new MapSingleCustomIdPropertyMap()));

            var type = typeof(DoubleIdEntity);
            var keyResolver = new Resolvers.DommelKeyPropertyResolver();
            var columnResolver = new Resolvers.DommelColumnNameResolver();
            FluentMapper.EntityMaps.TryGetValue(type, out var entityMap);

            var keys = keyResolver.ResolveKeyProperties(type);
            var columnName = columnResolver.ResolveColumnName(keys.Single().Property);

            var idName = columnResolver.ResolveColumnName(entityMap.PropertyMaps.Single(x => x.PropertyInfo.Name == nameof(DoubleIdEntity.Id)).PropertyInfo);

            Assert.Single(keys);
            Assert.Equal("id", columnName);
            Assert.Equal("Id", idName);
        }

        [Fact]
        public void EntityMapsToDefaultSingleKey()
        {
            FluentMapper.Initialize(c => c.AddMap(new MapSingleCustomIdDefaultKey()));

            var type = typeof(DoubleIdEntity);
            var keyResolver = new Resolvers.DommelKeyPropertyResolver();
            var keys = keyResolver.ResolveKeyProperties(type);
            Assert.Single(keys);
        }

        [Fact]
        public void KeyPropertyIsGenerated()
        {
            FluentMapper.Initialize(c => c.AddMap(new MapSingleCustomIdPropertyMap()));

            var type = typeof(DoubleIdEntity);
            var keyResolver = new Resolvers.DommelKeyPropertyResolver();
            var keys = keyResolver.ResolveKeyProperties(type);

            var key = keys.FirstOrDefault();
            Assert.True(key.IsGenerated);
        }

        [Fact]
        public void PropertyIsGenerated()
        {
            FluentMapper.Initialize(c => c.AddMap(new MapSingleCustomIdPropertyMap()));

            var type = typeof(DoubleIdEntity);
            var propertyResolver = new Resolvers.DommelPropertyResolver();
            var properties = propertyResolver.ResolveProperties(type);

            var property = properties.Where(x => x.IsGenerated);
            Assert.NotEmpty(property);

        }

        [Fact]
        public void EntityMapsToMultipleKeys()
        {
            FluentMapper.Initialize(c => c.AddMap(new MapCompositeKeyPropertyMap()));

            var type = typeof(CompositeKeyEntity);
            var keyResolver = new Resolvers.DommelKeyPropertyResolver();
            var keys = keyResolver.ResolveKeyProperties(type);

            Assert.True(keys.Count() == 2);
            Assert.All(keys, k => Assert.False(k.IsGenerated));
        }

        [Fact]
        public void PropertiesAreNotGenerated()
        {
            FluentMapper.Initialize(c => c.AddMap(new MapCompositeKeyPropertyMap()));

            var type = typeof(CompositeKeyEntity);
            var propertyResolver = new Resolvers.DommelPropertyResolver();
            var properties = propertyResolver.ResolveProperties(type);

            Assert.All(properties, p => Assert.False(p.IsGenerated));
        }

        [Fact]
        public void OnlyConventionApplied()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddConvention<SnakeCaseConvention>().ForEntity<TestEntity>();
                config.ForDommel();
            });

            var type = typeof(TestEntity);
            var keyResolver = new Resolvers.DommelKeyPropertyResolver();
            var columnResolver = new Resolvers.DommelColumnNameResolver();
            var entityMapExists = FluentMapper.EntityMaps.TryGetValue(type, out var entityMap);
            Assert.False(entityMapExists);

            var keys = keyResolver.ResolveKeyProperties(type);
            var keyColumnName = columnResolver.ResolveColumnName(keys.Single().Property);
            var idColumnName = columnResolver.ResolveColumnName(GetPropertyInfo<TestEntity>(_ => _.Id));
            var firstNameColumnName = columnResolver.ResolveColumnName(GetPropertyInfo<TestEntity>(_ => _.FirstName));
            var otherIdColumnName = columnResolver.ResolveColumnName(GetPropertyInfo<TestEntity>(_ => _.OtherId));

            Assert.Single(keys);
            Assert.Equal("id", keyColumnName);
            Assert.Equal("id", idColumnName);
            Assert.Equal("first_name", firstNameColumnName);
            Assert.Equal("other_id", otherIdColumnName);
        }

        [Fact]
        public void ConventionAndEntityMapApplied()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddConvention<SnakeCaseConvention>().ForEntity<TestEntity>();
                config.AddMap(new TestEntityMap());
                config.ForDommel();
            });

            var type = typeof(TestEntity);
            var keyResolver = new Resolvers.DommelKeyPropertyResolver();
            var columnResolver = new Resolvers.DommelColumnNameResolver();
            var entityMapExists = FluentMapper.EntityMaps.TryGetValue(type, out var entityMap);
            Assert.True(entityMapExists);

            var keys = keyResolver.ResolveKeyProperties(type);
            var keyColumnName = columnResolver.ResolveColumnName(keys.Single().Property);
            var idColumnName = columnResolver.ResolveColumnName(GetPropertyInfo<TestEntity>(_ => _.Id));
            var firstNameColumnName = columnResolver.ResolveColumnName(GetPropertyInfo<TestEntity>(_ => _.FirstName));
            var otherIdColumnName = columnResolver.ResolveColumnName(GetPropertyInfo<TestEntity>(_ => _.OtherId));

            Assert.Single(keys);
            Assert.Equal("id", keyColumnName);
            Assert.Equal("id", idColumnName);
            Assert.Equal("first_name", firstNameColumnName);
            Assert.Equal("otherid", otherIdColumnName);
        }

        private static void PreTest()
        {
            FluentMapper.EntityMaps.Clear();
            FluentMapper.TypeConventions.Clear();
        }

        private static PropertyInfo GetPropertyInfo<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            var memberInfo = ReflectionHelper.GetMemberInfo(expression);
            return (PropertyInfo)memberInfo;
        }

        private class MapWithCustomIdPropertyMap : ConventionalDommelEntityMap<CustomIdEntity>
        {
            public MapWithCustomIdPropertyMap()
            {
                Map(p => p.CustomId).ToColumn("customid").IsIdentity().IsKey();
            }
        }

        private class MapSingleCustomIdPropertyMap : ConventionalDommelEntityMap<DoubleIdEntity>
        {
            public MapSingleCustomIdPropertyMap()
            {
                Map(p => p.Id);
                Map(p => p.CustomId).IsKey().ToColumn("id", false);
            }
        }

        private class MapSingleCustomIdDefaultKey : ConventionalDommelEntityMap<DoubleIdEntity>
        {
            public MapSingleCustomIdDefaultKey()
            {
                Map(p => p.CustomId).ToColumn("customid", false);
            }
        }

        private class MapCompositeKeyPropertyMap : ConventionalDommelEntityMap<CompositeKeyEntity>
        {
            public MapCompositeKeyPropertyMap()
            {
                Map(p => p.KeyPartOne).IsKey().SetGeneratedOption(DatabaseGeneratedOption.None);
                Map(p => p.KeyPartTwo).IsKey().SetGeneratedOption(DatabaseGeneratedOption.None);
            }
        }
        
        private class TestEntityMap : ConventionalDommelEntityMap<TestEntity>
        {
            public TestEntityMap()
            {
                Map(_ => _.OtherId).ToColumn("otherid");
            }
        }
    }
}