using Questor.Vehicle.Domain.Mutations.Brands.Entities;
using Questor.Vehicle.Domain.Mutations.Models.Entities;
using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Questor.Vehicle.Domain.Mutations.Vehicles.Entities
{
    [Table("vehicles")]
    public class Vehicle
    {
        [Required] [Key]
        public string Id { get; set; }
        
        [Required]
        public int Year { get; set; }

        [Required] [ForeignKey("id_fuel")]
        public EFuel Fuel { get; set; }

        [Required] [ForeignKey("id_color")]
        public EColor Color { get; set; }

        [Required] [ForeignKey("id_brand")]
        public Brand Brand { get; set; }

        [Required] [ForeignKey("id_model")]
        public Model Model { get; set; }
    }
}
