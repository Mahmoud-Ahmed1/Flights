﻿using Flights.ReadModels;
using Flights.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using Flights.Dtos;
using Flights.Domain.Errors;
using Flights.Data;
using Microsoft.EntityFrameworkCore;

namespace Flights.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly Entities _entities;

        private readonly ILogger<FlightController> _logger;
        public FlightController(ILogger<FlightController> logger,
            Entities entities)
        {
            _logger = logger;
            _entities = entities;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<FlightRm>), 200)]

        public IEnumerable<FlightRm> Search([FromQuery] FlightSearchParameters @params)
        {

            _logger.LogInformation("Searching for a flight for: {Destination}", @params.Destination);

            IQueryable<Flight> flights = _entities.Flights;

            if (!string.IsNullOrWhiteSpace(@params.Destination))
                flights = flights.Where(f => f.Arrival.Place.Contains(@params.Destination));

            if (!string.IsNullOrWhiteSpace(@params.From))
                flights = flights.Where(f => f.Departure.Place.Contains(@params.From));

            if (@params.FromDate != null)
                flights = flights.Where(f => f.Departure.Time >= @params.FromDate.Value.Date);

            if (@params.ToDate != null)
                flights = flights.Where(f => f.Departure.Time >= @params.ToDate.Value.Date.AddDays(1).AddTicks(-1));

            if (@params.NumberOfPassengers != 0 && @params.NumberOfPassengers != null)
                flights = flights.Where(f => f.ReaminingNumberOfSeats >= @params.NumberOfPassengers);
            else
                flights = flights.Where(f => f.ReaminingNumberOfSeats >= 1);


            var flightRmList = flights
                .Select(flight => new FlightRm(
                flight.Id,
                flight.Airline,
                flight.Price,
                new TimePlaceRm(flight.Departure.Place.ToString(), flight.Departure.Time),
                new TimePlaceRm(flight.Arrival.Place.ToString(), flight.Arrival.Time),
                flight.ReaminingNumberOfSeats
                ));

            return flightRmList;
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(FlightRm), 200)]
        [HttpGet("{id}")]
        public ActionResult<FlightRm> Find(Guid id)
        {
            var flight = _entities.Flights.SingleOrDefault(f => f.Id == id);

            if (flight == null)
                return NotFound();
            var readModel = new FlightRm(
                flight.Id,
                flight.Airline,
                flight.Price,
                new TimePlaceRm(flight.Departure.Place.ToString(), flight.Departure.Time),
                new TimePlaceRm(flight.Arrival.Place.ToString(), flight.Arrival.Time),
                flight.ReaminingNumberOfSeats
                );
            return Ok(readModel);

        }
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Book(BookDto dto)
        {
            System.Diagnostics.Debug.WriteLine($"Booking a new flight {dto.FlightId}");

            var flight = _entities.Flights.SingleOrDefault(f => f.Id == dto.FlightId);

            if (flight == null)
                return NotFound();

            var error = flight.MakeBooking(dto.PassengerEmail, dto.NumberOfSeats);

            if (error is OverbookError)
                return Conflict(new { message = " Not enough seats." });

            try
            {
                _entities.SaveChanges();
            } catch (DbUpdateConcurrencyException e)
            {
                return Conflict(new { message = " An error occurred while booking. please try again." });
            }

            return CreatedAtAction(nameof(Find), new { id = dto.FlightId });
        }
        [HttpPost("createFlight")]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CreateFlight(FlightDto flightdto)
        {
            var flight = new Flight(
                Guid.NewGuid(),
                flightdto.Airline,
                flightdto.Price,
                flightdto.Departure,
                flightdto.Arrival,
                flightdto.ReaminingNumberOfSeats
                );
            if(flight == null) 
                return NotFound();
            _entities.Flights.Add(flight);
            _entities.SaveChanges();
            return CreatedAtAction(nameof(Find), new { id = flight.Id});
        }
    }
}    
                 

    
