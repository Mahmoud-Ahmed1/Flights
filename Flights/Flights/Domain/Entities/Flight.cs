﻿using Flights.Domain.Errors;
using Flights.ReadModels;

namespace Flights.Domain.Entities
{
    public class Flight
    {
        public Guid Id { get; set; }
        public string Airline { get; set; }
        public string Price { get; set; }
        public TimePlace Departure { get; set; }
        public TimePlace Arrival { get; set; }
        public int ReaminingNumberOfSeats { get; set; }

        public IList<Booking> Bookings = new List<Booking>();

        public Flight() 
        {

        }

        public Flight(
        Guid id,
        string airline,
        string price,
        TimePlace departure,
        TimePlace arrival,
        int reaminingNumberOfSeats
        )
        {
            Id = id;
            Airline = airline;
            Price = price; 
            Departure = departure;
            Arrival = arrival;
            ReaminingNumberOfSeats = reaminingNumberOfSeats;
        }

        public object? MakeBooking(string passengerEmail , byte numberOfSeats)
        {
            var flight = this;
            if (flight.ReaminingNumberOfSeats < numberOfSeats)
            {
                return new OverbookError();
            }
            flight.Bookings.Add(
                new Booking(
                    passengerEmail,
                    numberOfSeats)
                );
            flight.ReaminingNumberOfSeats -= numberOfSeats;
            return null;
        }

        public object? CancelBooking(string passengerEmail, byte numberOfSeats)
        {
            var booking = Bookings.FirstOrDefault(b => numberOfSeats == b.NumberOfSeats
           && passengerEmail.ToLower() == b.PassengerEmail.ToLower());

            if (booking == null)
                return new NotFoundError();

            Bookings.Remove(booking);
            ReaminingNumberOfSeats += booking.NumberOfSeats;

            return null;
        }

    }
}
