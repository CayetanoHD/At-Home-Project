# 💱 Exchange App

A multi-API currency exchange application built with .NET that compares rates across multiple providers to find the best exchange rate for your currency conversions.

## 📁 Project Structure

```
At-Home-Project/
├── Src/
│   ├── ConsoleApp/
│   │   └── ExchangeApp.ConsoleApp.ExchangeConsole/
│   │       ├── Runner/
│   │       │   └── ConsoleRunner.cs
│   │       ├── Services/
│   │       │   └── ApiParentFallbackService.cs
│   │       └── Program.cs
│   │
│   ├── Core/
│   │   └── ExchangeApp.Core.Application/
│   │       ├── DTOs/
│   │       │   ├── Apis/
│   │       │   │   ├── Api1/
│   │       │   │   ├── Api2/
│   │       │   │   ├── Api3/
│   │       │   │   └── ApiCallResult.cs
│   │       │   ├── ExchangeRequestDto.cs
│   │       │   └── ExchangeResponseDto.cs
│   │       ├── Interfaces/
│   │       │   ├── Apis/
│   │       │   ├── Console/
│   │       │   ├── ExternalApi/
│   │       │   └── IBestRateService.cs
│   │       ├── Results/
│   │       │   └── Result.cs
│   │       └── Services/
│   │           ├── Api1/
│   │           ├── Api2/
│   │           ├── Api3/
│   │           └── BestRateService.cs
│   │
│   ├── Infrastructure/
│   │   └── ExchangeApp.Infrastructure.ExternalProviders/
│   │       └── ExternalCallServices/
│   │           ├── Api1ExchangeService.cs
│   │           ├── Api2ExchangeService.cs
│   │           └── Api3ExchangeService.cs
│   │
│   └── Test/
│       ├── ExchangeApp.UnitTest.ExchangeTest/
│       │   ├── Mocks/
│       │   ├── Services/
│       │   └── ServicesMock/
│       │
│       └── WebApis/
│           ├── ExchangeApp1/
│           ├── ExchangeApp2/
│           ├── ExchangeApp3/
│           └── ExchangeAppParent/
│
└── README.md
```

## ⚙️ How It Works

The console application sends a currency exchange request with the following format:

```json
{
  "sourceCurrency": "USD",
  "targetCurrency": "DOP",
  "amount": 100
}
```

The application then queries multiple exchange rate APIs, compares the results, and returns the best available rate.

## 🚀 Getting Started

### Prerequisites

- Visual Studio 2022 or later
- .NET 8.0 or later

### Running Multiple APIs Simultaneously

1. Open the solution in **Visual Studio**
2. Right-click on the solution in Solution Explorer
3. Select **"Set Startup Projects..."** (or **"Configurar proyecto de inicio"** in Spanish)
4. Enable the option **"Multiple startup projects"** (or **"Configurar múltiples proyectos de inicio"**)
5. A table will appear with columns: **Project** and **Action**
6. In the **Action** column, select **Start** for each API project you want to run:
   - ExchangeApp1
   - ExchangeApp2
   - ExchangeApp3
   - ExchangeAppParent
   - ExchangeApp.ConsoleApp.ExchangeConsole
7. Click **OK** to save the configuration
8. Press **F5** or click **Start** to run all projects simultaneously

## 💰 Supported Currencies

The application currently supports conversions between the following currencies:

- **USD** - United States Dollar
- **EUR** - Euro
- **DOP** - Dominican Peso

### Currency Validation

⚠️ **Important:** The system only allows conversions between USD, DOP, and EUR.

If you attempt to use any other currency, the system will return an error message indicating that only these three currencies are permitted.

**Example of valid requests:**
- USD → DOP
- EUR → USD
- DOP → EUR

**Example of invalid requests:**
- GBP → USD ❌
- USD → JPY ❌

## 🧪 Testing

The project includes comprehensive unit tests located in:

```
Test/ExchangeApp.UnitTest.ExchangeTest/
├── Mocks/
├── Services/
└── ServicesMock/
```

Run the tests using Visual Studio Test Explorer or with the following command:

```bash
dotnet test
```

## 🏗️ Architecture

The project follows Clean Architecture principles with clear separation of concerns:

- **ConsoleApp**: Entry point and console interface
- **Core.Application**: Business logic, DTOs, and service interfaces
- **Infrastructure**: External API integrations and data providers
- **Test**: Unit tests and mock web APIs


---

**Note:** Make sure all API projects are running before testing the console application to ensure proper rate comparison functionality.
