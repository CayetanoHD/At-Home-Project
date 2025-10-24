At-Home-Project/
Src/
 ├── ConsoleApp/
 │   └── ExchangeApp.ConsoleApp.ExchangeConsole/
 │       ├── Runner/
 │       │   └── ConsoleRunner.cs
 │       ├── Services/
 │       │   └── ApiParentFallbackService.cs
 │       └── Program.cs
 ├── Core/
 │   └── ExchangeApp.Core.Application/
 │       ├── DTOs/
 │       │   ├── Apis/
 │       │   │   ├── Api1/
 │       │   │   │   ├── Api1Request.cs
 │       │   │   │   └── Api1Response.cs
 │       │   │   ├── Api2/
 │       │   │   │   ├── Api2Request.cs
 │       │   │   │   └── Api2Response.cs
 │       │   │   ├── Api3/
 │       │   │   │   ├── Api3Request.cs
 │       │   │   │   └── Api3Response.cs
 │       │   │   └── ApiCallResult.cs
 │       │   ├── ExchangeRequestDto.cs
 │       │   └── ExchangeResponseDto.cs
 │       ├── Interfaces/
 │       │   ├── Apis/
 │       │   │   ├── IApi1Service.cs
 │       │   │   ├── IApi2Service.cs
 │       │   │   └── IApi3Service.cs
 │       │   ├── Console/
 │       │   │   └── IApiParentFallbackService.cs
 │       │   ├── ExternalApi/
 │       │   │   ├── IApi1ExchangeService.cs
 │       │   │   ├── IApi2ExchangeService.cs
 │       │   │   └── IApi3ExchangeService.cs
 │       │   └── IBestRateService.cs
 │       ├── Results/
 │       │   └── Result.cs
 │       └── Services/
 │           ├── Api1/
 │           │   └── Api1Service.cs
 │           ├── Api2/
 │           │   └── Api2Service.cs
 │           ├── Api3/
 │           │   └── Api3Service.cs
 │           └── BestRateService.cs
 ├── Infrastructure/
 │   └── ExchangeApp.Infrastructure.ExternalProviders/
 │       └── ExternalCallServices/
 │           ├── Api1ExchangeService.cs
 │           ├── Api2ExchangeService.cs
 │           └── Api3ExchangeService.cs
 └── Test/
 │     ├── ExchangeApp.UnitTest.ExchangeTest/
 │     │   ├── Mocks/
 │     │   │   ├── Api1ServiceMock.cs
 │     │   │   ├── Api2ServiceMock.cs
 │     │   │   └── Api3ServiceMock.cs
 │     │   ├── Services/
 │     │   │   ├── Api1ServiceTests.cs
 │     │   │   ├── Api2ServiceTests.cs
 │     │   │   └── Api3ServiceTests.cs
 │     │   └── ServicesMock/
 │     │       ├── Api1ServiceMockTest.cs
 │     │       ├── Api2ServiceMockTest.cs
 │     │       ├── Api3ServiceMockTest.cs
 │     │       └── BestRateServiceMockTest.cs
 │     └── WebApis/
 │         ├── ExchangeApp1/
 │         ├── ExchangeApp2/
 │         ├── ExchangeApp3/
 │         └── ExchangeAppParent/
 │
 │
 └── README.md

## ⚙️ How It Works

1. **Console app** sends one set of data  
   json
   { "sourceCurrency": "USD", "targetCurrency": "DOP", "amount": 100 }

------------------------ENGLISH--------------------------------------------

Running the Project with Multiple APIs

Open the solution in Visual Studio.

Right-click on the solution and select “Set Startup Projects…”.

Enable the option “Multiple startup projects”.

A table will appear with the headers Project and Action.

In the Action column, choose Start for each project you want to run when the solution starts.

This allows you to run multiple projects simultaneously when launching the solution.

Conversion Tests (Allowed Currencies)

The program only allows conversions between DOP, USD, and EUR.

If you try to use a different currency, the system will return an error stating that only USD, DOP, and EUR are allowed.


-------------------------ESPAÑOL--------------------------------------------

Para ejecutar el proyecto junto con sus diferentes APIs:

Abre la solución en Visual Studio.

Haz clic derecho sobre la solución y selecciona "Configurar proyecto de inicio".

Activa la opción "Configurar múltiples proyectos de inicio".

Aparecerá una tabla con los encabezados Project y Action.

En la columna Action, selecciona para cada proyecto si deseas que se ejecute al iniciar.

Con esto, podrás ejecutar varios proyectos al mismo tiempo cuando inicies la solución.

Pruebas de conversión (monedas permitidas)

El programa solo permite convertir entre DOP, USD y EUR.

Si intentas usar otra moneda, el sistema debe devolver un error indicando que solo se permiten USD, DOP y EUR.
