using web_api_1.Models;

namespace web_api_1.Data;

public static class InMemoryDatabase
{
    public static List<Room> Rooms { get; } = new()
    {
        new Room
        {
            Id = 1,
            Name = "Sala 101",
            BuildingCode = "A",
            Floor = 1,
            Capacity = 20,
            HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 2,
            Name = "Lab 204",
            BuildingCode = "B",
            Floor = 2,
            Capacity = 24,
            HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 3,
            Name = "Sala 305",
            BuildingCode = "A",
            Floor = 3,
            Capacity = 35,
            HasProjector = false,
            IsActive = true
        },
        new Room
        {
            Id = 4,
            Name = "Sala 12",
            BuildingCode = "C",
            Floor = 0,
            Capacity = 15,
            HasProjector = false,
            IsActive = false
        },
        new Room
        {
            Id = 5,
            Name = "Audytorium",
            BuildingCode = "B",
            Floor = 1,
            Capacity = 80,
            HasProjector = true,
            IsActive = true
        }
    };

    public static List<Reservation> Reservations { get; } = new()
    {
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Jan Nowak",
            Topic = "Wprowadzenie do C#",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(10, 30),
            Status = "planned"
        },
        new Reservation
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
        new Reservation
        {
            Id = 3,
            RoomId = 3,
            OrganizerName = "Piotr Zielinski",
            Topic = "Konsultacje projektowe",
            Date = new DateOnly(2026, 5, 11),
            StartTime = new TimeOnly(13, 0),
            EndTime = new TimeOnly(14, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 4,
            RoomId = 5,
            OrganizerName = "Maria Wisniewska",
            Topic = "Szkolenie z baz danych",
            Date = new DateOnly(2026, 5, 12),
            StartTime = new TimeOnly(8, 30),
            EndTime = new TimeOnly(11, 0),
            Status = "planned"
        },
        new Reservation
        {
            Id = 5,
            RoomId = 1,
            OrganizerName = "Tomasz Lewandowski",
            Topic = "Powtórka przed kolokwium",
            Date = new DateOnly(2026, 5, 13),
            StartTime = new TimeOnly(15, 0),
            EndTime = new TimeOnly(16, 30),
            Status = "cancelled"
        }
    };
}