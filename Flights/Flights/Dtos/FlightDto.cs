using Flights.Domain.Entities;
using Flights.ReadModels;
using System.ComponentModel.DataAnnotations;

namespace Flights.Dtos
{
    public record FlightDto
        (
        string Airline,
        string Price,
        TimePlace Departure,
        TimePlace Arrival,
        int ReaminingNumberOfSeats
        );
}
