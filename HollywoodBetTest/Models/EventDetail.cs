using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HollywoodBetTest.Models
{
    public class EventDetail
    {
        [Key]
        public long EventDetailID { get; set; }

        [ForeignKey("Event")]
        public long EventID { get; set; }
        public Event Event { get; set; }


        [ForeignKey("EventDetailStatus")]
        public Int16 EventDetailStatusID { get; set; }
        public EventDetailStatus EventDetailStatus { get; set; }


        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string EventDetailName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 7)")]
        public decimal EventDetailOdd { get; set; }

        public Int16 FinishingPosition { get; set; }
    }
}
