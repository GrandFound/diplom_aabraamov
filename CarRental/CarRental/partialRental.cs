using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental
{
    partial class Rental
    {
        public bool FullPayment
        {
            get
            {
                if (RentalCost == PaymentStamp) return true;
                else return false;
            }
            set { }
        }
    }
}
