using Questor.Vehicle.Domain.Mutations.Brands.Entities;
using Questor.Vehicle.Domain.Mutations.Models.Entities;
using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Questor.Vehicle.Domain.Utils.Random;
using Questor.Vehicle.Domain.Mutations.Announcements.Entities;
using System;

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

        [Column("photo_date")]
        public DateTime? PhotoDate { get; private set; }

        [Required] [Column("id_model")] [ForeignKey("Model")]
        public string ModelId { get; private set; }
        public virtual Model Model { get; private set; }
        
        public virtual Announcement Announcement { get; private set; }

        public Vehicle() { }
        public Vehicle(
            string id,
            int year,
            EFuel fuel,
            EColor color,
            string modelId,
            DateTime? photoDate,
            Model model = null
        ) : this() {
            this.Id = string.IsNullOrWhiteSpace(id) ? RandomId.NewId() : id;
            this.SetData(
                year: year,
                fuel: fuel,
                color: color,
                modelId: modelId,
                photoDate: photoDate,
                model: model
            );
        }

        public void SetData(
            int year,
            EFuel fuel,
            EColor color,
            string modelId,
            DateTime? photoDate,
            Model model = null
        ) {
            this.Year = year;
            this.Fuel = fuel;
            this.Color = color;
            this.ModelId = modelId;
            this.PhotoDate = photoDate;
            this.Model = model;
        }

        public void Sell(DateTime dateSale) { 
            if (Announcement == null) return;
            Announcement.SetDateSale(dateSale);
        }
    }
}
