using Microsoft.EntityFrameworkCore.ChangeTracking;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Mutations.Brands.Entities;
using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle.UnitTests
{
    public class EntitiesFactory
    {
        private readonly VehicleMutationsDbContext DbContext;

        public EntitiesFactory(VehicleMutationsDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public BuilderFactory<Brand> NewBrand() { 
            var brand = new Brand {
                Id = RandomId.NewId(),
                Name = RandomId.NewId(150)
            };

            return new BuilderFactory<Brand>(brand, DbContext);
        }
    }

    public class BuilderFactory<TModel> where TModel : class {
        private readonly VehicleMutationsDbContext DbContext;
        private readonly TModel Model;

        public BuilderFactory(
            TModel model,
            VehicleMutationsDbContext dbContext
        ) {
            DbContext = dbContext;
            Model = model;
        }

        public TModel Get() => Model;

        public TModel Save() {
            DbContext.AddAsync(Model);
            DbContext.SaveChangesAsync();
            return Model;
        }
    }
}
