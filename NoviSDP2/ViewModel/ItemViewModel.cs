using NoviSDP2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.ViewModel
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Owner { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool Available { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public Item Item { get; set; }
        public int Days { get; set; }
        public string Status { get; set; }
        public string Borowwer { get; set; }
        public string Description { get; set; }
    }

}
