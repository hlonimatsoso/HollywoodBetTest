using System;
using System.Collections.Generic;
using System.Text;

namespace HollywoodBetTest.Models
{
    public class UserAccess
    {
        public bool tournaments_read { get; set; }
        public bool tournaments_write { get; set; }
        public bool tournaments_delete { get; set; }
        public bool events_read { get; set; }
        public bool events_write { get; set; }
        public bool events_delete { get; set; }
        public bool horses_read { get; set; }
        public bool horses_write { get; set; }
        public bool horses_delete { get; set; }


    }
}
