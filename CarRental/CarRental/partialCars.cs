using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static CarRental.AppData;

namespace CarRental
{
    partial class Cars
    {
        public string Status
        {
            get
            {
                int k = 0;
                List<Rental> rentalList = Rental.Where(x => x.CarID == ID).ToList();
                foreach (Rental rent in rentalList)
                {
                    if (rent.DateOfIssue <= DateTime.Today && rent.ReturnDate >= DateTime.Today) k++;
                }
                if (k == 0) return "Свободен";
                else return "В прокате";
            }
            set { }
        }
    }
}
