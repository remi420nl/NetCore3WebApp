﻿using NoviSDP2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Interface
{
    public interface ICheckoutRepository
    {
        IEnumerable<Checkout> GetAll();
        Checkout Get(int id);
        void Add(Checkout checkout);
        void CheckoutItem(int id);
        void CheckInItem(int id);
        bool IsCheckedOut(int id);



    }
}
