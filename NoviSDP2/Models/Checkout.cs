using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Models
{
    public class Checkout
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public DateTime From { get; set; }
        public DateTime Until { get; set; }

    }
}
