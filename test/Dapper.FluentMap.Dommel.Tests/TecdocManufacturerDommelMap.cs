using System.ComponentModel.DataAnnotations.Schema;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Dommel.Tests.Domain;

namespace Dapper.FluentMap.Dommel.Tests
{
    public class TecdocManufacturerDommelMap : ConventionalDommelEntityMap<TecdocManufacturer>
    {
        public TecdocManufacturerDommelMap()
        {
            Map(_ => _.Id)
                .ToColumn(nameof(TecdocManufacturer.Id).ToSnakeCase())
                .IsKey()
                .SetGeneratedOption(DatabaseGeneratedOption.None);
            // Map(_ => _.Name).ToColumn(nameof(TecdocManufacturer.Name).ToSnakeCase());
            // Map(_ => _.CreatedAt).ToColumn(nameof(TecdocManufacturer.CreatedAt).ToSnakeCase());
            // Map(_ => _.UpdatedAt).ToColumn(nameof(TecdocManufacturer.UpdatedAt).ToSnakeCase());
        }
    }
}