# HotelSearch

HotelSearch is a proof-of-concept ASP.NET Core Web API for hotel data management and hotel search.

The application provides CRUD operations for hotels and a search endpoint that returns hotels ordered by a combination of price and distance from a selected location.

## Technologies

- C#
- ASP.NET Core Web API
- Swagger / OpenAPI
- xUnit
- GitHub Actions

## Features

- CRUD operations for hotels
- In-memory data storage
- Search by user prompt
- Budget extraction from prompt
- Location extraction for predefined cities
- Distance calculation using the Haversine formula
- Ordering by price and distance
- Paging support
- Input validation
- Unit tests
- Health check endpoint
- Automated build and test workflow

## Project structure

```text
LemaxHotelSearch
├── .github
│   └── workflows
│       └── dotnet.yml
├── HotelSearch
│   ├── Controllers
│   ├── DTO
│   ├── Helpers
│   ├── Models
│   ├── Repositories
│   ├── Services
│   └── Program.cs
└── HotelSearch.Tests
    ├── DistanceCalculatorTests.cs
    ├── HotelSearchServiceTests.cs
    └── SimplePromptParserTests.cs
```

## How to run

1. Open the solution in Visual Studio.
2. Set `HotelSearch` as the startup project.
3. Run the application.
4. Use Swagger UI to test the API.

Alternatively:

```bash
dotnet run --project HotelSearch/HotelSearch.csproj
```

Run tests with:

```bash
dotnet test HotelSearch.Tests/HotelSearch.Tests.csproj
```

## API endpoints

```http
GET /api/Hotels
GET /api/Hotels/{id}
POST /api/Hotels
PUT /api/Hotels/{id}
DELETE /api/Hotels/{id}
POST /api/Hotels/search
GET /health
```

## Create hotel example

```json
{
  "name": "Hotel Dubrovnik",
  "price": 120,
  "latitude": 45.8129,
  "longitude": 15.9771
}
```

## Search example

```json
{
  "prompt": "Find hotels in Zagreb under 150",
  "currentLatitude": 45.815,
  "currentLongitude": 15.9819,
  "page": 1,
  "pageSize": 10
}
```

Example response:

```json
{
  "items": [
    {
      "name": "Hotel Zagreb Center",
      "price": 90,
      "distanceKm": 0.21
    },
    {
      "name": "Hotel Dubrovnik",
      "price": 120,
      "distanceKm": 0.44
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 2
}
```

## Search logic

The search endpoint parses the user prompt and extracts:

- a maximum budget from phrases such as `under 150`, `below 150`, `max 150`, `maximum 150`, and `up to 150`
- a location for predefined cities such as Zagreb, Split, Rijeka, and Oslo

If a known city is found in the prompt, its coordinates are used. Otherwise, the coordinates sent in the request are used.

Distance is calculated using the Haversine formula.

Results are ordered using a simple weighted score:

```text
score = price + distanceKm * 10
```

Hotels with a lower score are returned first.

## Technical design

The application is separated into controllers, services, repositories, DTOs, models, and helper classes.

- Controllers handle HTTP requests and responses.
- Services contain search and prompt parsing logic.
- Repositories handle data access.
- DTOs define API request and response models.
- Models represent hotel and location data.

The current repository stores data in memory. Data access is hidden behind the `IHotelRepository` interface, so a database-backed implementation can be added later without changing the controller or search service.

## Performance

For `n` hotels:

- filtering and distance calculation are O(n)
- sorting is O(n log n)
- paging is applied after sorting

The overall search complexity is O(n log n), mainly because of sorting.

For a production system with a larger data set, filtering and paging should be moved to the persistence layer and supported with appropriate database indexes.

## Validation and security

The API validates:

- hotel name
- hotel price
- latitude
- longitude
- search prompt
- page
- page size

Invalid requests return `400 Bad Request`.

Authentication and authorization are not included because this is a proof-of-concept assignment. They would be required before exposing the API in a production environment.

## Tests

The solution includes unit tests for:

- distance calculation
- prompt parsing
- budget filtering
- paging
- search result ordering

Tests can be run from Visual Studio Test Explorer or with:

```bash
dotnet test HotelSearch.Tests/HotelSearch.Tests.csproj
```

## CI and health checks

The repository includes a GitHub Actions workflow that restores dependencies, builds the project, and runs tests on pushes and pull requests to `main`.

A basic health check is available at:

```http
GET /health
```

## Use of AI tools

AI tools were used as support for:

- clarifying the assignment requirements
- discussing the project structure
- identifying edge cases
- reviewing API design
- improving documentation

The implementation was reviewed and adjusted manually. The final solution was intentionally kept simple and understandable while still allowing future extension.

## Possible improvements

- Add persistent database storage
- Add authentication and authorization
- Replace the predefined location parser with a geocoding service or LLM-based parser
- Add structured application logging
- Add integration tests
- Add containerization and deployment steps