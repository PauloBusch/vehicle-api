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

        [Required] [ForeignKey("id_fuel")]
        public EFuel Fuel { get; private set; }

        [Required] [ForeignKey("id_color")]
        public EColor Color { get; private set; }

        [Required] [ForeignKey("id_brand")]
        public string BrandId { get; private set; }
        public virtual Brand Brand { get; private set; }

        [Required] [ForeignKey("id_model")]
        public string ModelId { get; private set; }
        public virtual Model Model { get; private set; }

        public Vehicle() { }
        public Vehicle(
            string id,
            int year,
            EFuel fuel,
            EColor color,
            string brandId,
            string modelId
        ) {
            this.Id = string.IsNullOrWhiteSpace(id) ? RandomId.NewId() : id;
            this.SetData(
                year: year,
                fuel: fuel,
                color: color,
                brandId: brandId,
                modelId: modelId
            );
        }

        public void SetData(
            int year,
            EFuel fuel,
            EColor color,
            string brandId,
            string modelId
        ) {
            this.Year = year;
            this.Fuel = fuel;
            this.Color = color;
            this.BrandId = brandId;
            this.ModelId = modelId;
        }
    }
}
