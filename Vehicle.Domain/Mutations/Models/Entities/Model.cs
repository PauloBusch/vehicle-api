using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Questor.Vehicle.Domain.Mutations.Models.Entities
{
    [Table("models")]
    public class Model
    {
        [Requered] [Key]
        public string Id { get; set; }

        [Requered] [MaxLength(200)] [Index("UQ_models_name", IsUnique = true)]
        public string Name { get; set; }
    }
}
