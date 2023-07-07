using System;
using System.Collections.Generic;
using System.IO;

namespace Traveless.Data
{
    public class FlightManager
    {
        public List<Flight> FindFlights(string originAirport, string destinationAirport, string dayOfWeek)
        {
            List<Flight> matchingFlights = new List<Flight>();

            string filePath = Path.Combine(AppContext.BaseDirectory, @"resources/flights.csv");
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] fields = line.Split(',');
                string csvflightNumber = fields[0];
                string csvairline = fields[1];
                string csvOriginAirport = fields[2];
                string csvDestinationAirport = fields[3];
                string csvDayOfWeek = fields[4];
                string csvdepartureTime = fields[5];
                string csvPrice = fields[6];

                if (csvOriginAirport == originAirport && csvDestinationAirport == destinationAirport && csvDayOfWeek == dayOfWeek)
                {
                    matchingFlights.Add(new Flight
                    {
                        FlightNumber = csvflightNumber,
                        Airline = csvairline,
                        OriginAirport = csvOriginAirport,
                        DestinationAirport = csvDestinationAirport,
                        DayOfWeek = csvDayOfWeek,
                        DepartureTime = csvdepartureTime,
                        Price = csvPrice
                    });
                }
            }

            return matchingFlights;
        }
    }
}		