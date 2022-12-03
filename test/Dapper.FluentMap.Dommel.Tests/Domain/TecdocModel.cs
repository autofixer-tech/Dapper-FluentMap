using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper.FluentMap.Dommel.Tests.Domain
{
    [Table("tecdoc_models", Schema = "tecdoc")]
    public class TecdocModel : Entity<long>
    {
        public TecdocModel(
            long id,
            long tecdocManufacturerId,
            string name,
            bool isFavoured,
            string vehicleTypeId,
            DateTime? constructionFrom,
            DateTime? constructionTo)
        {
            Id = id;
            TecdocManufacturerId = tecdocManufacturerId;
            Name = name;
            IsFavoured = isFavoured;
            VehicleTypeId = vehicleTypeId;
            ConstructionFrom = constructionFrom;
            ConstructionTo = constructionTo;
        }

        private TecdocModel()
        {
        }

        public string Name { get; private set; }

        public long TecdocManufacturerId { get; private set; }

        public TecdocManufacturer TecdocManufacturer { get; private set; }

        public string VehicleTypeId { get; private set; }

        public TecdocVehicleType TecdocVehicleType { get; private set; }

        public bool IsFavoured { get; private set; }

        public DateTime? ConstructionFrom { get; private set; }

        public DateTime? ConstructionTo { get; private set; }

        public void ChangeManufacturer(TecdocManufacturer tecdocManufacturer)
        {
            AssignToManufacturer(tecdocManufacturer);
            tecdocManufacturer?.AssignModel(this);
        }

        internal void AssignToManufacturer(TecdocManufacturer tecdocManufacturer)
        {
            if (tecdocManufacturer == null)
            {
                throw new Exception("Tecdoc manufacturer must not be null");
            }

            TecdocManufacturerId = tecdocManufacturer.Id;
            TecdocManufacturer = tecdocManufacturer;
        }
    }
}