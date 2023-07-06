using System;
using System.Collections.Generic;
using System.IO;
using Traveless.Data;
using System.Text;

namespace Traveless.Data
{
    public class ReservationManager
    {
        private List<Reservation> reservations;
        private string csvFilePath;

        public ReservationManager(string csvFilePath)
        {
            reservations = new List<Reservation>();
            this.csvFilePath = csvFilePath;
            LoadReservationsFromCsv();
        }

        public List<Reservation> FindReservations(string reservationCode, string airline, string name)
        {
            List<Reservation> matchingReservations = new List<Reservation>();
            var csvFilePath = Path.Combine(AppContext.BaseDirectory, @"resources\reservations.csv");
            string[] lines = File.ReadAllLines(csvFilePath);
            foreach (string line in lines)
            {
                string[] fields = line.Split(',');
                string csvReservationCode = fields[0];
                string csvName = fields[1];
                string csvCitizenship = fields[2];
                string csvFlightNumber = fields[3];
                string csvAirline = fields[4];
                string csvDay = fields[5];
                string csvDepartureTime = fields[6];
                string csvPrice = fields[5];

                string csvStatus = fields[8];

                if ((string.IsNullOrEmpty(reservationCode) || csvReservationCode.Contains(reservationCode)) &&
                    (string.IsNullOrEmpty(airline) || csvAirline.Contains(airline)) &&
                    (string.IsNullOrEmpty(name) || csvName.Contains(name)))
                {
                    matchingReservations.Add(new Reservation
                    {
                        ReservationCode = csvReservationCode,
                        FlightNumber = csvFlightNumber,
                        Airline = csvAirline,
                        Name = csvName,
                        Citizenship = csvCitizenship,
                        Status = csvStatus,
                        DayOfWeek = csvDay,
                        DepartureTime = csvDepartureTime,
                        Price = csvPrice
                    });
                }
            }

            return matchingReservations;
        }

        public void AddReservation(Reservation reservation)
        {
            reservations.Add(reservation);
            SaveReservationsToCsv();
        }
        private void LoadReservationsFromCsv()
        {
            if (File.Exists(csvFilePath))
            {
                string[] lines = File.ReadAllLines(csvFilePath);
                foreach (string line in lines)
                {
                    string[] fields = line.Split(',');
                    string csvReservationCode = fields[0];
                    string csvFlightNumber = fields[1];
                    string csvAirline = fields[2];
                    string csvDay = fields[3];
                    string csvDepartureTime = fields[4];
                    string csvPrice = fields[5];
                    string csvName = fields[6];
                    string csvCitizenship = fields[7];
                    string csvStatus = fields[8];

                    reservations.Add(new Reservation
                    {
                        ReservationCode = csvReservationCode,
                        FlightNumber = csvFlightNumber,
                        Airline = csvAirline,
                        Name = csvName,
                        Citizenship = csvCitizenship,
                        Status = csvStatus,
                        DayOfWeek = csvDay,
                        DepartureTime = csvDepartureTime,
                        Price = csvPrice
                    });
                }
            }
        }

        private void SaveReservationsToCsv()
        {
            StringBuilder csv = new StringBuilder();
            csv.AppendLine("ReservationCode,FlightNumber,Airline,DayOfWeek,DepartureTime,Price,Name,Citizenship,Status");

            foreach (var reservation in reservations)
            {
                csv.AppendLine($"{reservation.ReservationCode},{reservation.FlightNumber},{reservation.Airline},{reservation.DayOfWeek},{reservation.DepartureTime},{reservation.Price},{reservation.Name},{reservation.Citizenship},{reservation.Status}");
            }
            var csvFilePath = Path.Combine(AppContext.BaseDirectory, @"resources\reservations.csv");
            File.WriteAllText(csvFilePath, csv.ToString());
        }
    }
}
    
