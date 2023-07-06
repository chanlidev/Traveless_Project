using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Traveless.Data
{
    public class Reservation
    {
        public string ReservationCode { get; set; }
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string Price { get; set; }
        public string Name { get; set; }
        public string Citizenship { get; set; }
        public string Status { get; set; }
        public string DayOfWeek { get; set; }
        public string DepartureTime { get; set; }

    }
}