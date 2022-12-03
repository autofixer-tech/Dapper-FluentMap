using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dapper.FluentMap.Dommel.Tests.Domain
{
    [Table("tecdoc_manufacturers", Schema = "tecdoc")]
    public class TecdocManufacturer : Entity<long>
    {
        private readonly List<TecdocModel> _tecdocModels = new List<TecdocModel>();

        public TecdocManufacturer(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public TecdocManufacturer()
        {
        }

        public IReadOnlyCollection<TecdocModel> TecdocModels => _tecdocModels;

        public string Name { get; private set; }

        public void AddModel(TecdocModel tecdocModel)
        {
            AssignModel(tecdocModel);
            tecdocModel.AssignToManufacturer(this);
        }

        internal void AssignModel(TecdocModel tecdocModel)
        {
            if (tecdocModel == null)
            {
                throw new Exception("Tecdoc model must not be null");
            }

            if (_tecdocModels.Any(_ => _.Id == tecdocModel.Id))
            {
                throw new Exception("Tecdoc model with id {TecdocModelId} already exists");
            }

            _tecdocModels.Add(tecdocModel);
        }
    }
}