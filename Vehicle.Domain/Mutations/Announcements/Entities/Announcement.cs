using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Questor.Vehicle.Domain.Mutations.Announcements.Entities
{
    [Table("announcements")]
    public class Announcement
    {
        [Required] [Key]
        public string Id { get; private set; }
        
        [Required] [Column("price_purchase")]
        public decimal PricePurchase { get; private set; }

        [Required] [Column("price_sale")]
        public decimal PriceSale { get; private set; }

        [Column("date_sale")] [Index("IDX_announcements_date_sale")]
        public DateTime? DateSale { get; private set; }

        [Required] [Column("id_vehicle")] [ForeignKey("Vehicle")]
        public string VehicleId { get; private set; }
        public virtual Vehicles.Entities.Vehicle Vehicle { get; private set; }

        public Announcement() { }

        public Announcement(
            string id,
            decimal pricePurchase,
            decimal priceSale,
            DateTime? dateSale,
            string vehicleId,
            Vehicles.Entities.Vehicle vehicle = null
        ) : this() {
            this.Id = string.IsNullOrWhiteSpace(id) ? RandomId.NewId() : id;
            this.SetData(
                pricePurchase: pricePurchase,
                priceSale: priceSale,
                dateSale: dateSale,
                vehicleId: vehicleId,
                vehicle: vehicle
            );
        }

        public void SetData(
            decimal pricePurchase,
            decimal priceSale,
            DateTime? dateSale,
            string vehicleId,
            Vehicles.Entities.Vehicle vehicle = null
        ) { 
            this.PricePurchase = pricePurchase;
            this.PriceSale = priceSale;
            this.DateSale = dateSale;
            this.VehicleId = vehicleId;
            this.Vehicle = vehicle;
        }

        public void SetDateSale(DateTime dateSale)
        {
            this.DateSale = dateSale;
        }
    }
}
