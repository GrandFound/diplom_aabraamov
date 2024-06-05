using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental
{
    internal class AppData
    {
        private static CarRentalDiplomEntities1 _context;

        public static CarRentalDiplomEntities1 GetContext()
        {
            if (_context == null)
                _context = new CarRentalDiplomEntities1();
            return _context;
        }
    }
}
