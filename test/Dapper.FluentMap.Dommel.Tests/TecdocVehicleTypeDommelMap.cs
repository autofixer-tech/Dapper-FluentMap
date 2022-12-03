using System.ComponentModel.DataAnnotations.Schema;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Dommel.Tests.Domain;

namespace Dapper.FluentMap.Dommel.Tests
{
    public class TecdocVehicleTypeDommelMap : ConventionalDommelEntityMap<TecdocVehicleType>
    {
        public TecdocVehicleTypeDommelMap()
        {
            Map(_ => _.Id)
                .ToColumn(nameof(TecdocVehicleType.Id).ToSnakeCase())
                .IsKey()
                .SetGeneratedOption(DatabaseGeneratedOption.None);
            // Map(_ => _.Name).ToColumn(nameof(TecdocVehicleType.Name).ToSnakeCase());
            // Map(_ => _.CreatedAt).ToColumn(nameof(TecdocVehicleType.CreatedAt).ToSnakeCase());
            // Map(_ => _.UpdatedAt).ToColumn(nameof(TecdocVehicleType.UpdatedAt).ToSnakeCase());
        }
    }
}