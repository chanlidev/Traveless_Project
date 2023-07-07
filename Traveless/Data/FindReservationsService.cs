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
            string csvFilePath = Path.Combine(AppContext.BaseDirectory, @"resources/reservations.csv");
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
                string csvPrice = fields[7];
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

        public void UpdateReservation(Reservation reservation)
        {
            var existingReservation = reservations.Find(r => r.ReservationCode == reservation.ReservationCode);
            if (existingReservation != null)
            {
                // Check if the name, citizenship, or status is changed
                bool isNameChanged = existingReservation.Name != reservation.Name;
                bool isCitizenshipChanged = existingReservation.Citizenship != reservation.Citizenship;
                bool isStatusChanged = existingReservation.Status != reservation.Status;

                if (isNameChanged || isCitizenshipChanged || isStatusChanged)
                {
                    // Soft-delete the existing flight reservation by marking it as inactive
                    existingReservation.Status = "Inactive";

                    // Create a new reservation record with the updated values
                    var updatedReservation = new Reservation
                    {
                        ReservationCode = reservation.ReservationCode,
                        FlightNumber = existingReservation.FlightNumber,
                        Airline = existingReservation.Airline,
                        Name = reservation.Name,
                        Citizenship = reservation.Citizenship,
                        Status = reservation.Status,
                        DayOfWeek = existingReservation.DayOfWeek,
                        DepartureTime = existingReservation.DepartureTime,
                        Price = existingReservation.Price
                    };

                    // Add the updated reservation to the list
                    reservations.Add(updatedReservation);
                }
                else
                {
                    // Update the reservation with the new values
                    existingReservation.Name = reservation.Name;
                    existingReservation.Citizenship = reservation.Citizenship;
                    existingReservation.Status = reservation.Status;
                }

                SaveReservationsToCsv();
            }
            else
            {
                throw new Exception("Reservation not found.");
            }
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
                    string csvName = fields[1];
                    string csvCitizenship = fields[2];
                    string csvFlightNumber = fields[3];
                    string csvAirline = fields[4];
                    string csvDay = fields[5];
                    string csvDepartureTime = fields[6];
                    string csvPrice = fields[7];
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

        public void SaveReservationsToCsv()
        {
            StringBuilder csv = new StringBuilder();
            csv.AppendLine("ReservationCode,Name,Citizenship,FlightNumber,Airline,DayOfWeek,DepartureTime,Price,Status");

            foreach (var reservation in reservations)
            {
                // Add the reservation to the CSV
                csv.AppendLine($"{reservation.ReservationCode},{reservation.Name},{reservation.Citizenship},{reservation.FlightNumber},{reservation.Airline},{reservation.DayOfWeek},{reservation.DepartureTime},{reservation.Price},{reservation.Status}");

                // Add additional logic to save updated reservations as new records
                if (reservation.Status == "Inactive")
                {
                    var updatedReservation = new Reservation
                    {
                        ReservationCode = reservation.ReservationCode,
                        FlightNumber = reservation.FlightNumber,
                        Airline = reservation.Airline,
                        Name = reservation.Name,
                        Citizenship = reservation.Citizenship,
                        Status = reservation.Status,
                        DayOfWeek = reservation.DayOfWeek,
                        DepartureTime = reservation.DepartureTime,
                        Price = reservation.Price
                    };
                    csv.AppendLine($"{updatedReservation.ReservationCode},{updatedReservation.Name},{updatedReservation.Citizenship},{updatedReservation.FlightNumber},{updatedReservation.Airline},{updatedReservation.DayOfWeek},{updatedReservation.DepartureTime},{updatedReservation.Price},{updatedReservation.Status}");
                }
            }
            string csvFilePath = Path.Combine(AppContext.BaseDirectory, @"resources/reservations.csv");
            File.WriteAllText(csvFilePath, csv.ToString());
        }


    }
}
    
