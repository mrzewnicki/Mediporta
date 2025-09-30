# Struktura projektów

Poniżej znajduje się opis głównych katalogów i projektów w solucji:

- Client\Mediporta.Client.App
  - Aplikacja kliencka Blazor WebAssembly (front‑end).
  - Zawiera komponenty, strony, layouty oraz statyczne zasoby w wwwroot.
- Client\Tests\Mediporta.Client.Tests.Unit
  - Testy jednostkowe dla warstwy klienckiej.

- Mediporta.Shared
  - Wspólna biblioteka współdzielona między klientem i serwerem.
  - Zawiera DTOs, Helpers oraz Querying

- Server\Mediporta.Api
  - ASP.NET Core Web API – punkt wejścia backendu.

- Server\Mediporta.Core
  - Logika domenowa/aplikacyjna, wzorzec CQRS.
  - Foldery Command/Queries z implementacjami zapytań i komend.

- Server\Mediporta.Data
  - Dostęp do danych (Entity Framework Core): Entities, Repositories, Migrations, Configuration.
  - baza SQLite

- Server\Mediporta.StackOverflow.ApiClient
  - Integracja z zewnętrznym API (StackOverflow) – modele i klient API.

- Server\Tests\Mediporta.Tests.Unit
  - Testy jednostkowe dla warstwy serwerowej.
- Server\Tests\Mediporta.Tests.Integration
  - Testy integracyjne backendu.

- compose.yaml
  - Konfiguracja Docker Compose dla uruchomienia środowiska lokalnego (API + Frontend).

Uwagi dodatkowe
- Środowisko docelowe: .NET 9 (net9.0).
- Uruchomienie całości: `docker compose up --build`
- Po uruchomieniu:
  - Backend (API): http://localhost:5080/api/
  - OpenAPI (manifest.json): http://localhost:5080/openapi/v1.json
  - Frontend: http://localhost:8085/
