﻿using System;
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
            LoadReservationsFromCsv(reservations);
        }

        public List<Reservation> FindReservations(string reservationCode, string airline, string name)
        {
            List<Reservation> matchingReservations = new List<Reservation>();
            string documentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string csvFilePath = Path.Combine(documentsFolderPath, "reservations.csv");
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
                        Name = csvName,
                        Citizenship = csvCitizenship,
                        FlightNumber = csvFlightNumber,
                        Airline = csvAirline,
                        DayOfWeek = csvDay,
                        DepartureTime = csvDepartureTime,
                        Price = csvPrice,
                        Status = csvStatus   
                    });
                    
                }
            }

            return matchingReservations;
        }

        public void AddReservation(Reservation reservation)
        {
            reservations.Add(reservation);
            SaveReservationsToCsv(reservations);
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
                    SaveReservationsToCsv(reservations); // Save the updated status to the CSV file

                    // Create a new reservation record with the updated values
                    var updatedReservation = new Reservation
                    {
                        ReservationCode = Guid.NewGuid().ToString(),
                        Name = reservation.Name,
                        Citizenship = reservation.Citizenship,
                        FlightNumber = existingReservation.FlightNumber,
                        Airline = existingReservation.Airline,
                        DayOfWeek = existingReservation.DayOfWeek,
                        DepartureTime = existingReservation.DepartureTime,
                        Price = existingReservation.Price,
                        Status = reservation.Status
                    };

                    // Add the updated reservation as a new record
                    reservations.Add(updatedReservation);
                    SaveReservationsToCsv(reservations);
                }
                else
                {
                    // Update the reservation with the new values
                    existingReservation.Name = reservation.Name;
                    existingReservation.Citizenship = reservation.Citizenship;
                    existingReservation.Status = reservation.Status;
                    SaveReservationsToCsv(reservations);
                }
            }
            else
            {
                throw new Exception("Reservation not found.");
            }
        }

        public List<Reservation> GetReservations()
        {
            return reservations;
        }

        private void LoadReservationsFromCsv(List<Reservation> reservations)
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
                        Name = csvName,
                        Citizenship = csvCitizenship,
                        FlightNumber = csvFlightNumber,
                        Airline = csvAirline,
                        DayOfWeek = csvDay,
                        DepartureTime = csvDepartureTime,
                        Price = csvPrice,
                        Status = csvStatus,
                        
                    });

                }
            }
        }

        public void SaveReservationsToCsv(List<Reservation> reservations)
        {
            StringBuilder csv = new StringBuilder();
            csv.AppendLine("ReservationCode,Name,Citizenship,FlightNumber,Airline,DayOfWeek,DepartureTime,Price,Status");

            foreach (var reservation in reservations)
            {

                csv.AppendLine($"{reservation.ReservationCode},{reservation.Name},{reservation.Citizenship},{reservation.FlightNumber},{reservation.Airline},{reservation.DayOfWeek},{reservation.DepartureTime},{reservation.Price},{reservation.Status}");
            }

            string documentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string csvFilePath = Path.Combine(documentsFolderPath, "reservations.csv");
            File.WriteAllText(csvFilePath, csv.ToString());

        }
    }
}
    
