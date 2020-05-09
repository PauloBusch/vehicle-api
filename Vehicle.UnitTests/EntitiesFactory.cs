using Microsoft.EntityFrameworkCore.ChangeTracking;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Mutations.Announcements.Entities;
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
            string brandId = null,
            int? year = null,
            EFuel? fuel = null,
            EColor? color = null
        ) {
            var model = NewModel(id: modelId).Get();
            var brand = NewBrand(id: brandId).Get();
            var vehicle = new Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Vehicle(
                id: id ?? RandomId.NewId(),
                year: year ?? 2010,
                fuel: fuel ?? EFuel.Gasoline,
                color: color ?? EColor.Brown,
                modelId: model.Id,
                brandId: brand.Id,
                model: model,
                brand: brand
            );

            return new BuilderFactory<Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Vehicle>(vehicle, DbContext);
        }

        public BuilderFactory<Announcement> NewAnnouncement(
            string id = null,
            string vehicleId = null,
            DateTime? dateSale = null,
            Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Vehicle vehicle = null
        ) {
            var vehicleData = vehicle ?? NewVehicle(id: vehicleId).Get();
            var announcement = new Announcement(
                id: id,
                pricePurchase: StaticRandom.Next(10000, 50000),
                priceSale: StaticRandom.Next(50000, 80000),
                dateSale: dateSale,
                vehicleId: vehicleData.Id,
                vehicle: vehicleData
            );

            return new BuilderFactory<Announcement>(announcement, DbContext);
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
            DbContext.Add(Model);
            DbContext.SaveChanges();
            return Model;
        }
    }
}
