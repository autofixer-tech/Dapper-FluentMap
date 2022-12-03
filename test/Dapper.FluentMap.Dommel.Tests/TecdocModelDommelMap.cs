using System.ComponentModel.DataAnnotations.Schema;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Dommel.Tests.Domain;

namespace Dapper.FluentMap.Dommel.Tests
{
    public class TecdocModelDommelMap : ConventionalDommelEntityMap<TecdocModel>
    {
        public TecdocModelDommelMap()
        {
            Map(_ => _.Id)
                .ToColumn(nameof(TecdocModel.Id).ToSnakeCase())
                .IsKey()
                .SetGeneratedOption(DatabaseGeneratedOption.None);
            // Map(_ => _.TecdocManufacturerId).ToColumn(nameof(TecdocModel.TecdocManufacturerId).ToSnakeCase());
            // Map(_ => _.Name).ToColumn(nameof(TecdocModel.Name).ToSnakeCase());
            // Map(_ => _.VehicleTypeId).ToColumn(nameof(TecdocModel.VehicleTypeId).ToSnakeCase());
            // Map(_ => _.IsFavoured).ToColumn(nameof(TecdocModel.IsFavoured).ToSnakeCase());
            // Map(_ => _.ConstructionFrom).ToColumn(nameof(TecdocModel.ConstructionFrom).ToSnakeCase());
            // Map(_ => _.ConstructionTo).ToColumn(nameof(TecdocModel.ConstructionTo).ToSnakeCase());
            // Map(_ => _.CreatedAt).ToColumn(nameof(TecdocModel.CreatedAt).ToSnakeCase());
            // Map(_ => _.UpdatedAt).ToColumn(nameof(TecdocModel.UpdatedAt).ToSnakeCase());
        }
    }
}