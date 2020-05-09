using Questor.Vehicle.Domain.Mutations.Brands.Entities;
using Questor.Vehicle.Domain.Mutations.Models.Entities;
using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Questor.Vehicle.Domain.Utils.Random;

namespace Questor.Vehicle.Domain.Mutations.Vehicles.Entities
{
    [Table("vehicles")]
    public class Vehicle
    {
        [Required] [Key]
        public string Id { get; private set; }
        
        [Required]
        public int Year { get; private set; }

        [Required] [Column("id_fuel")] [ForeignKey("Fuel")]
        public EFuel Fuel { get; private set; }

        [Required] [Column("id_color")] [ForeignKey("Color")]
        public EColor Color { get; private set; }

        [Required] [Column("id_brand")] [ForeignKey("Brand")]
        public string BrandId { get; private set; }
        public virtual Brand Brand { get; private set; }

        [Required] [Column("id_model")] [ForeignKey("Model")]
        public string ModelId { get; private set; }
        public virtual Model Model { get; private set; }

        public Vehicle() { }
        public Vehicle(
            string id,
            int year,
            EFuel fuel,
            EColor color,
            string brandId,
            string modelId,
            Brand brand = null,
            Model model = null
        ) : this() {
            this.Id = string.IsNullOrWhiteSpace(id) ? RandomId.NewId() : id;
            this.SetData(
                year: year,
                fuel: fuel,
                color: color,
                brandId: brandId,
                modelId: modelId,
                brand: brand,
                model: model
            );
        }

        public void SetData(
            int year,
            EFuel fuel,
            EColor color,
            string brandId,
            string modelId,
            Brand brand = null,
            Model model = null
        ) {
            this.Year = year;
            this.Fuel = fuel;
            this.Color = color;
            this.BrandId = brandId;
            this.ModelId = modelId;
            this.Model = model;
            this.Brand = brand;
        }
    }
}
