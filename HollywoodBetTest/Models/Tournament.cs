using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HollywoodBetTest.Models
{
    public class Tournament
    {
        [Key]
        public long TournamentID { get; set; }

        [Required]
        [MaxLength(200)]
        [Column(TypeName = "varchar(200)")]
        public string TournamentName { get; set; }
    }
}
