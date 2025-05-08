# SkyBooker 

SkyBooker ist eine moderne Microservice-Anwendung zur Verwaltung von Flugreservierungen mit ASP.NET Core, Docker, RabbitMQ und einer API-Gateway-Struktur.

## Technologien

- **.NET 8** – WebAPI-Entwicklung
- **Docker & Docker Compose** – Containerisierung aller Services
- **RabbitMQ** – Asynchrone Kommunikation (Erweiterung Level 3)
- **MongoDB** – NoSQL-Datenbank für Fluginformationen
- **SQL Server** – Relationale Datenbank für Buchungen
- **Swagger / Ocelot** – Dokumentation & API-Gateway
- **JWT Authentication** – Sicherheitsmechanismus für alle Endpunkte
- **FluentValidation** – Eingabevalidierung
- **XUnit / Moq** – Unit Testing
- **REST Client (HTTP-Files)** – API-Test direkt in VS Code

## Architektur

```
[Client] → [API Gateway] → [AuthService]
                       → [FlightService]
                       → [BookService] → [RabbitMQ] → [MessageService]
```

## Services

| Service         | Port  | Funktion                                      |
|-----------------|-------|-----------------------------------------------|
| **AuthService** | 5001  | Registrierung & JWT-Login                     |
| **FlightService** | 5002  | Verwaltung von Flugverbindungen (MongoDB)    |
| **BookService** | 5003  | Verwaltung von Buchungen (SQL Server)         |
| **MessageService** | 5004  | Empfang von Buchungen via RabbitMQ           |
| **ApiGateway**  | 5000  | Zentraler Einstiegspunkt (Ocelot)             |
| **RabbitMQ UI** | 15672 | Admin-Panel für Messaging                     |

## Projekt ausführen

```bash
docker compose up --build
```

Frontend (Swagger) für jeden Dienst ist über seinen Port erreichbar, z. B.:

- http://localhost:5000/swagger

## Authentifizierung

1. POST `/api/auth/register`
2. POST `/api/auth/login`
3. Erhalte JWT-Token → Nutze als Header für geschützte Routen:

```
Authorization: Bearer <dein_token>
```

## Asynchrone Kommunikation

- `BookService` sendet Buchungsdetails via RabbitMQ
- `MessageService` empfängt Nachricht und simuliert:
  - WhatsApp-Benachrichtigung
  - Twilio-SMS

## Tests

```bash
dotnet test Services/BookingService.Tests
```

Testet Validierung und Controllerlogik mit `xUnit` & `Moq`.

## 👤 Autor

- **Name:** Nevan Alberola  
- **Projektzeitraum:** 30.04.2025 – 08.05.2025  
