using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Questor.Vehicle.Domain.Mutations.Reservations.Entities
{
    public class Contact
    {
        [Required] [Key]
        public string Id { get; private set; }
        
        [Required] [MaxLength(150)] [Index("IDX_contacts_name")]
        public string Name { get; private set; }

        [Required] [MaxLength(15)] [Index("UQ_contacts_phone", IsUnique = true)]
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
