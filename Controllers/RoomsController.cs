using Microsoft.AspNetCore.Mvc;
using TrainingCenterApi.Data;
using TrainingCenterApi.Models;

namespace TrainingCenterApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetRooms(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool activeOnly = false)
    {
        IEnumerable<Room> rooms = InMemoryData.Rooms;

        if (minCapacity.HasValue)
        {
            rooms = rooms.Where(room => room.Capacity >= minCapacity.Value);
        }

        if (hasProjector.HasValue)
        {
            rooms = rooms.Where(room => room.HasProjector == hasProjector.Value);
        }

        if (activeOnly)
        {
            rooms = rooms.Where(room => room.IsActive);
        }

        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Room> GetRoomById([FromRoute] int id)
    {
        var room = InMemoryData.Rooms.FirstOrDefault(room => room.Id == id);

        if (room is null)
        {
            return NotFound($"Sala o id {id} nie istnieje.");
        }

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public ActionResult<IEnumerable<Room>> GetRoomsByBuilding([FromRoute] string buildingCode)
    {
        var rooms = InMemoryData.Rooms
            .Where(room => room.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }

    [HttpPost]
    public ActionResult<Room> CreateRoom([FromBody] Room room)
    {
        lock (InMemoryData.SyncRoot)
        {
            room.Id = InMemoryData.NextRoomId++;
            InMemoryData.Rooms.Add(room);
        }

        return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Room> UpdateRoom([FromRoute] int id, [FromBody] Room updatedRoom)
    {
        var room = InMemoryData.Rooms.FirstOrDefault(room => room.Id == id);

        if (room is null)
        {
            return NotFound($"Sala o id {id} nie istnieje.");
        }

        room.Name = updatedRoom.Name;
        room.BuildingCode = updatedRoom.BuildingCode;
        room.Floor = updatedRoom.Floor;
        room.Capacity = updatedRoom.Capacity;
        room.HasProjector = updatedRoom.HasProjector;
        room.IsActive = updatedRoom.IsActive;

        return Ok(room);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteRoom([FromRoute] int id)
    {
        var room = InMemoryData.Rooms.FirstOrDefault(room => room.Id == id);

        if (room is null)
        {
            return NotFound($"Sala o id {id} nie istnieje.");
        }

        var hasReservations = InMemoryData.Reservations.Any(reservation => reservation.RoomId == id);
        if (hasReservations)
        {
            return Conflict("Nie można usunąć sali, ponieważ istnieją dla niej powiązane rezerwacje.");
        }

        InMemoryData.Rooms.Remove(room);
        return NoContent();
    }
}
