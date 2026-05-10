using Microsoft.AspNetCore.Mvc;
using TrainingCenterApi.Data;
using TrainingCenterApi.Models;

namespace TrainingCenterApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetReservations(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        IEnumerable<Reservation> reservations = InMemoryData.Reservations;

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
    public ActionResult<Reservation> GetReservationById([FromRoute] int id)
    {
        var reservation = InMemoryData.Reservations.FirstOrDefault(reservation => reservation.Id == id);

        if (reservation is null)
        {
            return NotFound($"Rezerwacja o id {id} nie istnieje.");
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> CreateReservation([FromBody] Reservation reservation)
    {
        var roomValidationResult = ValidateRoomForReservation(reservation.RoomId);
        if (roomValidationResult is not null)
        {
            return roomValidationResult;
        }

        if (HasTimeConflict(reservation))
        {
            return Conflict("Rezerwacja koliduje czasowo z istniejącą rezerwacją tej samej sali.");
        }

        lock (InMemoryData.SyncRoot)
        {
            reservation.Id = InMemoryData.NextReservationId++;
            InMemoryData.Reservations.Add(reservation);
        }

        return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Reservation> UpdateReservation([FromRoute] int id, [FromBody] Reservation updatedReservation)
    {
        var reservation = InMemoryData.Reservations.FirstOrDefault(reservation => reservation.Id == id);

        if (reservation is null)
        {
            return NotFound($"Rezerwacja o id {id} nie istnieje.");
        }

        var roomValidationResult = ValidateRoomForReservation(updatedReservation.RoomId);
        if (roomValidationResult is not null)
        {
            return roomValidationResult;
        }

        updatedReservation.Id = id;
        if (HasTimeConflict(updatedReservation, id))
        {
            return Conflict("Rezerwacja koliduje czasowo z istniejącą rezerwacją tej samej sali.");
        }

        reservation.RoomId = updatedReservation.RoomId;
        reservation.OrganizerName = updatedReservation.OrganizerName;
        reservation.Topic = updatedReservation.Topic;
        reservation.Date = updatedReservation.Date;
        reservation.StartTime = updatedReservation.StartTime;
        reservation.EndTime = updatedReservation.EndTime;
        reservation.Status = updatedReservation.Status;

        return Ok(reservation);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteReservation([FromRoute] int id)
    {
        var reservation = InMemoryData.Reservations.FirstOrDefault(reservation => reservation.Id == id);

        if (reservation is null)
        {
            return NotFound($"Rezerwacja o id {id} nie istnieje.");
        }

        InMemoryData.Reservations.Remove(reservation);
        return NoContent();
    }

    private static ActionResult? ValidateRoomForReservation(int roomId)
    {
        var room = InMemoryData.Rooms.FirstOrDefault(room => room.Id == roomId);

        if (room is null)
        {
            return new BadRequestObjectResult($"Nie można utworzyć rezerwacji. Sala o id {roomId} nie istnieje.");
        }

        if (!room.IsActive)
        {
            return new BadRequestObjectResult($"Nie można utworzyć rezerwacji. Sala o id {roomId} jest nieaktywna.");
        }

        return null;
    }

    private static bool HasTimeConflict(Reservation checkedReservation, int? ignoredReservationId = null)
    {
        return InMemoryData.Reservations.Any(existingReservation =>
            existingReservation.Id != ignoredReservationId &&
            existingReservation.RoomId == checkedReservation.RoomId &&
            existingReservation.Date == checkedReservation.Date &&
            !existingReservation.Status.Equals("cancelled", StringComparison.OrdinalIgnoreCase) &&
            !checkedReservation.Status.Equals("cancelled", StringComparison.OrdinalIgnoreCase) &&
            checkedReservation.StartTime < existingReservation.EndTime &&
            checkedReservation.EndTime > existingReservation.StartTime);
    }
}
