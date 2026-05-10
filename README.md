# TrainingCenterApi

Prosta aplikacja ASP.NET Core Web API oparta na kontrolerach. Dane sal i rezerwacji są przechowywane w statycznych listach w pamięci aplikacji.

## Uruchomienie

```bash
dotnet restore
dotnet run
```

Domyślnie projekt działa pod adresem:

- `http://localhost:5062`
- `https://localhost:7160`

## Endpointy sal

- `GET /api/rooms`
- `GET /api/rooms/{id}`
- `GET /api/rooms/building/{buildingCode}`
- `GET /api/rooms?minCapacity=20&hasProjector=true&activeOnly=true`
- `POST /api/rooms`
- `PUT /api/rooms/{id}`
- `DELETE /api/rooms/{id}`

## Endpointy rezerwacji

- `GET /api/reservations`
- `GET /api/reservations/{id}`
- `GET /api/reservations?date=2026-05-10&status=confirmed&roomId=2`
- `POST /api/reservations`
- `PUT /api/reservations/{id}`
- `DELETE /api/reservations/{id}`

## Przykład POST /api/rooms

```json
{
  "name": "Lab 204",
  "buildingCode": "B",
  "floor": 2,
  "capacity": 24,
  "hasProjector": true,
  "isActive": true
}
```

## Przykład POST /api/reservations

```json
{
  "roomId": 2,
  "organizerName": "Anna Kowalska",
  "topic": "Warsztaty z HTTP i REST",
  "date": "2026-05-10",
  "startTime": "13:00:00",
  "endTime": "14:30:00",
  "status": "confirmed"
}
```

Próba dodania rezerwacji dla sali 2 w dniu `2026-05-10` w godzinach `10:30:00-11:30:00` zwróci `409 Conflict`, ponieważ koliduje z przykładową rezerwacją `10:00:00-12:30:00`.
