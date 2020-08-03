using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Models
{
    public class Donation
    {
        public string Id { get; set; }

        [DisplayName("Donatie")]
        public decimal Total { get; set; }

        [DisplayName("Student Naam")]
        public string Student { get; set; }

        [DisplayName("Naam Ontvanger")]
        public string Name { get; set; }

    }
}
