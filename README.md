# Fora Financial - SEC EDGAR API Integration

This project implements a solution for the Fora Financial coding challenge, which imports company financial data from the SEC's EDGAR API and calculates loan eligibility amounts based on specific business rules.

## Overview

The application provides two main API endpoints:
1. **Import Companies** - Fetches company data from SEC EDGAR API and persists it to SQL Server
2. **Query Companies** - Retrieves companies with calculated funding amounts

## Technology Stack

- **.NET 10**: Latest framework
- **FastEndpoints**: REPR pattern for API endpoints
- **Entity Framework Core**: Data access with SQL Server
- **Aspire**: Cloud-ready orchestration and observability
- **Serilog**: Structured logging

## Getting Started

### Prerequisites

- .NET 10 SDK
- Docker (for SQL Server via Aspire)

### Running the Application

#### Option 1: Run with Aspire (Recommended)

```bash
dotnet run --project src/Fora.Seeker.AspireHost
```

The Aspire dashboard will open at `http://localhost:15888` where you can:
- View logs and traces
- Monitor the SQL Server container
- Access the web application

The web API will be available at `https://localhost:7165`

#### Option 2: Run Web Project Directly

```bash
# Apply migrations
dotnet ef database update -c AppDbContext -p src/Fora.Seeker.Web -s src/Fora.Seeker.Web

# Run the application
dotnet run --project src/Fora.Seeker.Web
```

## API Endpoints

### Import Companies

**POST** `/Companies/Import`

Imports company financial data from SEC EDGAR API.

**Request Body (Optional):**
```json
{
  "ciks": [18926, 892553, 1510524]
}
```

If no CIKs are provided, imports all 101 default CIKs from the requirements.

**Response:**
```json
{
  "totalProcessed": 3,
  "successCount": 3,
  "failureCount": 0,
  "errors": []
}
```

### Get Companies

**GET** `/Companies?startsWithLetter={letter}`

Retrieves companies with calculated funding amounts.

**Query Parameters:**
- `startsWithLetter` (optional): Filter companies by first letter of name

**Response:**
```json
[
  {
    "id": 1,
    "name": "UBER TECHNOLOGIES, INC",
    "standardFundableAmount": 1234567.89,
    "specialFundableAmount": 1420752.07
  }
]
```

## Business Rules

### Standard Fundable Amount

1. Company must have income data for all years 2018-2022
2. Company must have positive income in both 2021 and 2022
3. Based on highest income between 2018-2022:
   - If ≥ $10B: 12.33% of income
   - If < $10B: 21.51% of income

### Special Fundable Amount

Starts with Standard Fundable Amount, then:
- **+15%** if company name starts with a vowel (A, E, I, O, U)
- **-25%** if 2022 income < 2021 income

## Project Structure

```
src/Fora.Seeker.Web/
├── Domain/
│   └── CompanyAggregate/
│       ├── Company.cs              # Company entity
│       ├── CompanyIncome.cs        # Income data entity
│       └── FundingCalculator.cs    # Business logic for funding calculations
├── Infrastructure/
│   ├── Data/
│   │   ├── AppDbContext.cs         # EF Core context
│   │   ├── Config/                 # EF Core configurations
│   │   └── Migrations/             # Database migrations
│   └── Edgar/
│       ├── EdgarApiService.cs      # SEC API integration
│       ├── IEdgarApiService.cs     # Service interface
│       └── EdgarCompanyInfo.cs     # API response DTOs
└── CompanyFeatures/
    ├── ImportCompanies.cs          # Import endpoint
    ├── GetCompanies.cs             # Query endpoint
    └── CompanyResponse.cs          # Response DTO
```

## Database Schema

### Companies Table
- `Id` (PK)
- `Cik` (Unique)
- `Name`

### CompanyIncomes Table
- `Id` (PK)
- `CompanyId` (FK)
- `Year`
- `IncomeAmount`
- `Form` (e.g., "10-K")
- `Frame` (e.g., "CY2021")

## Testing

Use the included `api.http` file with VS Code REST Client or similar tools:

```http
### Import all companies
POST https://localhost:7165/Companies/Import
Content-Type: application/json

{}

### Get all companies
GET https://localhost:7165/Companies

### Get companies starting with 'A'
GET https://localhost:7165/Companies?startsWithLetter=A
```

## Development Notes

- The SEC EDGAR API requires specific headers: `User-Agent: PostmanRuntime/7.34.0` and `Accept: */*`
- Only 10-K forms with yearly data (Frame format "CY####") are imported
- A small delay (100ms) is added between API calls to avoid overwhelming the SEC API
- All funding amounts are rounded to 2 decimal places

## Production Considerations

For a production deployment, consider:
1. **Rate Limiting**: Implement proper rate limiting for SEC API calls
2. **Retry Logic**: Add exponential backoff for failed API requests
3. **Caching**: Cache company data to reduce API calls
4. **Background Jobs**: Move import process to background job queue
5. **Authentication**: Add API authentication/authorization
6. **Monitoring**: Set up alerts for import failures
7. **Data Validation**: Add more robust validation for income data

---

**Built for Fora Financial Coding Challenge**
