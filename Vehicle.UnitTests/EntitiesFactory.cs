using Microsoft.EntityFrameworkCore.ChangeTracking;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Mutations.Brands.Entities;
using Questor.Vehicle.Domain.Mutations.Models.Entities;
using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
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

        public BuilderFactory<Brand> NewBrand(
            string id = null,
            string name = null
        ) { 
            var brand = new Brand(
                id: id,
                name: name ?? RandomId.NewId(150)
            );

            return new BuilderFactory<Brand>(brand, DbContext);
        }

        public BuilderFactory<Model> NewModel(
            string id = null,
            string name = null
        ) { 
            var model = new Model(
                id: id,
                name: name ?? RandomId.NewId(150)
            );

            return new BuilderFactory<Model>(model, DbContext);
        }

        public BuilderFactory<Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Vehicle> NewVehicle(
            string id = null,
            string modelId = null,
            string brandId = null
        ) {
            var model = NewModel(id: modelId).Get();
            var brand = NewBrand(id: brandId).Get();
            var vehicle = new Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Vehicle(
                id: id ?? RandomId.NewId(),
                year: 2010,
                fuel: EFuel.Flex,
                color: EColor.Blue,
                modelId: model.Id,
                brandId: brand.Id,
                model: model,
                brand: brand
            );

            return new BuilderFactory<Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Vehicle>(vehicle, DbContext);
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
