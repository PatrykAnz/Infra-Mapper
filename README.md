# Infra-Mapper
## School project

PatrykAnz – 96160

Projekt natywnej aplikacji chmurowej do wizualizacji urządzeń infrastruktury, ich lokalizacji oraz połączeń.

## Stos technologiczny

- Presentation Layer: React + Vite
- Application Layer: Node.js 24 + NestJS (REST API)
- Data Layer: PostgreSQL / Azure Database for PostgreSQL
- Konteneryzacja: Docker
- Chmura: Microsoft Azure


## Deklaracja Architektury 

Ten projekt został zaplanowany w architekturze cloud-native z wykorzystaniem usług PaaS w Microsoft Azure.

| Warstwa        | Komponent lokalny          | Usługa Azure                    |
|----------------|----------------------------|---------------------------------|
| Presentation   | React + Vite               | Azure Static Web Apps           |
| Application    | API (Node.js 24, NestJS)   | Azure App Service               |
| Data           | PostgreSQL                 | Azure Database for PostgreSQL   |

## 🚦 Status Projektu

* [x] **Artefakt 1:** Architektura i struktura folderów.
* [x] **Artefakt 2:** Środowisko wielokontenerowe uruchomione lokalnie (Docker Compose).
* [x] **Artefakt 3:** Docker Compose environment for frontend
* [x] **Artefakt 4:** Działająca warstwa logiki backendu
* [x] **Artefakt 5:** Trwałość danych i profesjonalny kontrakt API (EF Migrations + DTO + UI Form).

## 🚀 Quick Start (Local EF)

Jeśli uruchamiasz projekt po raz pierwszy z bazą danych, wykonaj w terminalu:

**Uruchomienie infrastruktury:**

```bash
docker compose up -d
cd backend
dotnet ef database update
```
