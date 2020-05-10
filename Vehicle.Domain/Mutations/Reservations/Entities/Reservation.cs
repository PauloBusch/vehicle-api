
using Questor.Vehicle.Domain.Mutations.Announcements.Entities;
using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Questor.Vehicle.Domain.Mutations.Reservations.Entities
{
    public class Reservation
    {
        [Required] [Key]
        public string Id { get; private set; }

        [Required] [Column("id_contact")] [ForeignKey("Contact")]
        public string ContactId { get; private set; }
        public virtual Contact Contact { get; private set; }

        [Required] [Column("id_announcement")] [ForeignKey("Announcement")]
        public string AnnouncementId { get; private set; }
        public Announcement Announcement { get; private set; }

        public Reservation() { }

        public Reservation(
            string id,
            string contactId,
            string announcementId,
            Contact contact = null,
            Announcement announcement = null
        ) : this()
        {
            this.Id = string.IsNullOrWhiteSpace(id) ? RandomId.NewId() : id;
            this.SetData(
                contactId: contactId,
                announcementId: announcementId,
                contact: contact,
                announcement: announcement
            );
        }

        public void SetData(
            string contactId,
            string announcementId,
            Contact contact = null,
            Announcement announcement = null
        ) {
            this.ContactId = contactId;
            this.AnnouncementId = announcementId;
            this.Contact = contact;
            this.Announcement = announcement;
        }
    }
}
