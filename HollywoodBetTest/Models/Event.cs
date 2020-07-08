using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HollywoodBetTest.Models
{
    public class Event
    {
        [Key]
        public long EventID { get; set; }

        [ForeignKey("Tournament")]
        public long TournamentID { get; set; }

        public Tournament Tournament { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string EventName { get; set; }

        [Required]
        public Int16 EventNumber { get; set; }

        [Required]
        public DateTime EventDateTime { get; set; }

        [Required]
        public DateTime EventEndDateTime { get; set; }

        public bool AutoClose { get; set; }

    }
}
