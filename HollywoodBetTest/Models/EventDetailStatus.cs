using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HollywoodBetTest.Models
{
    public class EventDetailStatus
    {
        [Key]
        public Int16 EventDetailStatusID { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName ="varchar(50)")]
        public string EventDetailStatusName { get; set; }

    }
}
