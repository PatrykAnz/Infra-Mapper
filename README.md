# Infra-Mapper

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

## Status Projektu

* [x] **Artefakt 1:** Architektura i struktura folderów.
* [x] **Artefakt 2:** Środowisko wielokontenerowe uruchomione lokalnie (Docker Compose).
* [x] **Artefakt 3:** Docker Compose environment for frontend
