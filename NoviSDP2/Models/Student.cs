﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Models
{
    public class Student : Person
    {
        public override int Id { get; set; }
        public override string Name { get; set; }
        public IEnumerable<Checkout> Checkouts { get; set; }
        [EmailAddress]
        public override string Email { get; set; }

    }
}
