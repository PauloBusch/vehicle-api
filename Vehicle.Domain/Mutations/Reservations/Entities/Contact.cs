using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Questor.Vehicle.Domain.Mutations.Reservations.Entities
{
    public class Contact
    {
        [Required] [Key]
        public string Id { get; private set; }
        
        [Required] [MaxLength(150)]
        public string Name { get; private set; }

        [Required] [MaxLength(15)]
        public string Phone { get; private set; }

        public Contact() { }

        public Contact(
            string id,
            string name,
            string phone
        ) : this()
        {
            this.Id = string.IsNullOrWhiteSpace(id) ? RandomId.NewId() : id;
            this.SetData(
                name: name,
                phone: phone
            );
        }

        public void SetData(
            string name,
            string phone
        ) {
            this.Name = name;
            this.Phone = phone;
        }
    }
}
