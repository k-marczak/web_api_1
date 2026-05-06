using Microsoft.AspNetCore.Mvc;
using web_api_1.Data;
using web_api_1.Models;

namespace web_api_1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllReservations(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        var reservations = InMemoryDatabase.Reservations.AsEnumerable();

        if (date.HasValue)
        {
            reservations = reservations.Where(reservation => reservation.Date == date.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            reservations = reservations.Where(reservation =>
                reservation.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        if (roomId.HasValue)
        {
            reservations = reservations.Where(reservation => reservation.RoomId == roomId.Value);
        }

        return Ok(reservations);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetReservationById([FromRoute] int id)
    {
        var reservation = InMemoryDatabase.Reservations
            .FirstOrDefault(reservation => reservation.Id == id);

        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public IActionResult CreateReservation([FromBody] Reservation reservation)
    {
        var room = InMemoryDatabase.Rooms
            .FirstOrDefault(room => room.Id == reservation.RoomId);

        if (room is null)
        {
            return BadRequest("Room does not exist.");
        }

        if (!room.IsActive)
        {
            return BadRequest("Room is not active.");
        }

        var hasConflict = InMemoryDatabase.Reservations.Any(existingReservation =>
            existingReservation.RoomId == reservation.RoomId &&
            existingReservation.Date == reservation.Date &&
            existingReservation.Status != "cancelled" &&
            reservation.Status != "cancelled" &&
            reservation.StartTime < existingReservation.EndTime &&
            reservation.EndTime > existingReservation.StartTime
        );

        if (hasConflict)
        {
            return Conflict("Reservation conflicts with an existing reservation.");
        }

        reservation.Id = InMemoryDatabase.Reservations.Max(reservation => reservation.Id) + 1;

        InMemoryDatabase.Reservations.Add(reservation);

        return CreatedAtAction(
            nameof(GetReservationById),
            new { id = reservation.Id },
            reservation
        );
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateReservation([FromRoute] int id, [FromBody] Reservation updatedReservation)
    {
        var existingReservation = InMemoryDatabase.Reservations
            .FirstOrDefault(reservation => reservation.Id == id);

        if (existingReservation is null)
        {
            return NotFound();
        }

        var room = InMemoryDatabase.Rooms
            .FirstOrDefault(room => room.Id == updatedReservation.RoomId);

        if (room is null)
        {
            return BadRequest("Room does not exist.");
        }

        if (!room.IsActive)
        {
            return BadRequest("Room is not active.");
        }

        var hasConflict = InMemoryDatabase.Reservations.Any(reservation =>
            reservation.Id != id &&
            reservation.RoomId == updatedReservation.RoomId &&
            reservation.Date == updatedReservation.Date &&
            reservation.Status != "cancelled" &&
            updatedReservation.Status != "cancelled" &&
            updatedReservation.StartTime < reservation.EndTime &&
            updatedReservation.EndTime > reservation.StartTime
        );

        if (hasConflict)
        {
            return Conflict("Reservation conflicts with an existing reservation.");
        }

        existingReservation.RoomId = updatedReservation.RoomId;
        existingReservation.OrganizerName = updatedReservation.OrganizerName;
        existingReservation.Topic = updatedReservation.Topic;
        existingReservation.Date = updatedReservation.Date;
        existingReservation.StartTime = updatedReservation.StartTime;
        existingReservation.EndTime = updatedReservation.EndTime;
        existingReservation.Status = updatedReservation.Status;

        return Ok(existingReservation);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteReservation([FromRoute] int id)
    {
        var reservation = InMemoryDatabase.Reservations
            .FirstOrDefault(reservation => reservation.Id == id);

        if (reservation is null)
        {
            return NotFound();
        }

        InMemoryDatabase.Reservations.Remove(reservation);

        return NoContent();
    }
}