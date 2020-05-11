using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Models
{
    // Model class to Hold an item in other words a reservation
    public class Hold
    {

        public int Id { get; set; }
        public DateTime HoldDate { get; set; }
        public int chosenDays { get; set; }
        public Student Student { get; set; }
        public Item Item { get; set; }

        
    }



}
