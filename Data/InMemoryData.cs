using TrainingCenterApi.Models;

namespace TrainingCenterApi.Data;

public static class InMemoryData
{
    public static readonly object SyncRoot = new();

    public static List<Room> Rooms { get; private set; } = new();
    public static List<Reservation> Reservations { get; private set; } = new();

    public static int NextRoomId { get; set; }
    public static int NextReservationId { get; set; }

    public static void Initialize()
    {
        Rooms = new List<Room>
        {
            new()
            {
                Id = 1,
                Name = "Sala 101",
                BuildingCode = "A",
                Floor = 1,
                Capacity = 20,
                HasProjector = true,
                IsActive = true
            },
            new()
            {
                Id = 2,
                Name = "Lab 204",
                BuildingCode = "B",
                Floor = 2,
                Capacity = 24,
                HasProjector = true,
                IsActive = true
            },
            new()
            {
                Id = 3,
                Name = "Sala konferencyjna 12",
                BuildingCode = "A",
                Floor = 0,
                Capacity = 45,
                HasProjector = true,
                IsActive = true
            },
            new()
            {
                Id = 4,
                Name = "Sala 305",
                BuildingCode = "C",
                Floor = 3,
                Capacity = 16,
                HasProjector = false,
                IsActive = true
            },
            new()
            {
                Id = 5,
                Name = "Pracownia techniczna",
                BuildingCode = "B",
                Floor = 1,
                Capacity = 12,
                HasProjector = false,
                IsActive = false
            }
        };

        Reservations = new List<Reservation>
        {
            new()
            {
                Id = 1,
                RoomId = 1,
                OrganizerName = "Jan Nowak",
                Topic = "Konsultacje z programowania",
                Date = new DateOnly(2026, 5, 10),
                StartTime = new TimeOnly(8, 0),
                EndTime = new TimeOnly(9, 30),
                Status = "confirmed"
            },
            new()
            {
                Id = 2,
                RoomId = 2,
                OrganizerName = "Anna Kowalska",
                Topic = "Warsztaty z HTTP i REST",
                Date = new DateOnly(2026, 5, 10),
                StartTime = new TimeOnly(10, 0),
                EndTime = new TimeOnly(12, 30),
                Status = "confirmed"
            },
            new()
            {
                Id = 3,
                RoomId = 3,
                OrganizerName = "Piotr Zieliński",
                Topic = "Szkolenie z komunikacji",
                Date = new DateOnly(2026, 5, 11),
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(11, 0),
                Status = "planned"
            },
            new()
            {
                Id = 4,
                RoomId = 4,
                OrganizerName = "Maria Wiśniewska",
                Topic = "Konsultacje projektowe",
                Date = new DateOnly(2026, 5, 12),
                StartTime = new TimeOnly(13, 0),
                EndTime = new TimeOnly(14, 30),
                Status = "planned"
            },
            new()
            {
                Id = 5,
                RoomId = 2,
                OrganizerName = "Tomasz Wójcik",
                Topic = "Przegląd sprintu",
                Date = new DateOnly(2026, 5, 13),
                StartTime = new TimeOnly(15, 0),
                EndTime = new TimeOnly(16, 0),
                Status = "cancelled"
            }
        };

        NextRoomId = Rooms.Max(room => room.Id) + 1;
        NextReservationId = Reservations.Max(reservation => reservation.Id) + 1;
    }
}
