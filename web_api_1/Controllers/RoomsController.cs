using Microsoft.AspNetCore.Mvc;
using web_api_1.Data;
using web_api_1.Models;

namespace web_api_1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllRooms(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var rooms = InMemoryDatabase.Rooms.AsEnumerable();

        if (minCapacity.HasValue)
        {
            rooms = rooms.Where(room => room.Capacity >= minCapacity.Value);
        }

        if (hasProjector.HasValue)
        {
            rooms = rooms.Where(room => room.HasProjector == hasProjector.Value);
        }

        if (activeOnly == true)
        {
            rooms = rooms.Where(room => room.IsActive);
        }

        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetRoomById([FromRoute] int id)
    {
        var room = InMemoryDatabase.Rooms.FirstOrDefault(room => room.Id == id);

        if (room is null)
        {
            return NotFound();
        }

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetRoomsByBuildingCode([FromRoute] string buildingCode)
    {
        var rooms = InMemoryDatabase.Rooms
            .Where(room => room.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult CreateRoom([FromBody] Room room)
    {
        var newId = InMemoryDatabase.Rooms.Max(room => room.Id) + 1;

        room.Id = newId;

        InMemoryDatabase.Rooms.Add(room);

        return CreatedAtAction(
            nameof(GetRoomById),
            new { id = room.Id },
            room
        );
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateRoom([FromRoute] int id, [FromBody] Room updatedRoom)
    {
        var existingRoom = InMemoryDatabase.Rooms.FirstOrDefault(room => room.Id == id);

        if (existingRoom is null)
        {
            return NotFound();
        }

        existingRoom.Name = updatedRoom.Name;
        existingRoom.BuildingCode = updatedRoom.BuildingCode;
        existingRoom.Floor = updatedRoom.Floor;
        existingRoom.Capacity = updatedRoom.Capacity;
        existingRoom.HasProjector = updatedRoom.HasProjector;
        existingRoom.IsActive = updatedRoom.IsActive;

        return Ok(existingRoom);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteRoom([FromRoute] int id)
    {
        var room = InMemoryDatabase.Rooms.FirstOrDefault(room => room.Id == id);

        if (room is null)
        {
            return NotFound();
        }

        var hasReservations = InMemoryDatabase.Reservations
            .Any(reservation => reservation.RoomId == id);

        if (hasReservations)
        {
            return Conflict("Cannot delete room because it has reservations.");
        }

        InMemoryDatabase.Rooms.Remove(room);

        return NoContent();
    }
}