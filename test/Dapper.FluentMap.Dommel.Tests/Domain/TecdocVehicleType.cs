using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper.FluentMap.Dommel.Tests.Domain
{
    [Table("tecdoc_vehicle_types", Schema = "tecdoc")]
    public class TecdocVehicleType : Entity<string>
    {
        public TecdocVehicleType(string id, string name)
        {
            Id = id;
            Name = name;
        }

        private TecdocVehicleType()
        {
        }

        public string Name { get; private set; }
    }
}