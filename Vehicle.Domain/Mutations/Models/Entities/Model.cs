using Questor.Vehicle.Domain.Mutations.Brands.Entities;
using Questor.Vehicle.Domain.Utils.Random;
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
        [Required] [Key]
        public string Id { get; private set; }

        [Required] [MaxLength(200)] [Index("UQ_models_name", IsUnique = true)]
        public string Name { get; private set; }

        [Required] [Column("id_brand")] [ForeignKey("Brand")]
        public string BrandId { get; private set; }
        public virtual Brand Brand { get; private set; }

        public Model() { }

        public Model(
            string id,
            string name,
            string brandId,
            Brand brand = null
        ) : this() {
            this.Id = string.IsNullOrWhiteSpace(id) ? RandomId.NewId() : id;
            this.SetData(
                name: name,
                brandId: brandId,
                brand: brand
            );
        }

        public void SetData(
            string name,
            string brandId,
            Brand brand = null
        ) {
            this.Name = name;
            this.Brand = brand;
            this.BrandId = brandId;
        }
    }
}
