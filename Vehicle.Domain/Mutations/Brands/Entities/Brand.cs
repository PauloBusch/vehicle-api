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
        public string Id { get; private set; }

        [Required] [MaxLength(200)] [Index("UQ_brands_name", IsUnique = true)]
        public string Name { get; private set; }

        public Brand() { }

        public Brand(
            string id,
            string name
        ) : this()
        {
            this.Id = string.IsNullOrWhiteSpace(id) ? RandomId.NewId() : id;
            this.SetData(name: name);
        }

        public void SetData(string name)
        {
            this.Name = name;
        }
    }
}
