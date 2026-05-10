# APBD web api

## Opis projektu

Aplikacja Web Api oparta na kontrolerach. Dane są przechowywane w pamięci aplikacji. End Pointy zostały zweryfikowane w Postmanie. 

---

## Testy przeprowdzone w Postman 

GET http://localhost:5215/api/rooms/1
GET http://localhost:5215/api/rooms/building/A
GET http://localhost:5215/api/rooms?minCapacity=20&hasProjector=true&activeOnly=true
POST http://localhost:5215/api/rooms
PUT http://localhost:5215/api/rooms/1
POST http://localhost:5215/api/reservations
POST http://localhost:5215/api/reservations
DELETE http://localhost:5215/api/reservations/5
GET http://localhost:5215/api/rooms/999
