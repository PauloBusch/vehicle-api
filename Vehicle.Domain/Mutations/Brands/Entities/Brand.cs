using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Questor.Vehicle.Domain.Mutations.Brands.Entities
{
    [Table("brands")]
    public class Brand 
    {
        [Required] [Key]
        public string Id { get; set; }

        [Required] [MaxLength(200)] [Index("UQ_brands_name", IsUnique = true)]
        public string Name { get; set; }
    }
}
