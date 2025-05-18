# C# .NET Core - Web API - SQLite - React Meter Readings Project

Meter readings app with C# Web Api and React on the front end, that uploads a CSV file, processes the entries and saves them in the DB. UI is responsible to manage the file upload interface and rendering a list with all present entries.

* Uses C# .NET Core to create the APIs
* SQLite to set the DB
* Moq and xUnit to test the C# functionality
* React SPA to manage the file upload and render the meter readings

## About 

This project is a full-stack meter readings management system built with:

- **Backend:** .NET Core Web API (.NET 8)  
- **Database:** SQLite using Entity Framework Core  
- **Frontend:** React with TypeScript and Tailwind CSS  
- **Testing:** xUnit and Moq for .NET, React Testing Library and Jest for frontend

## Running locally in development mode

To get started, just clone the repository and run `dotnet restore & dotnet build & dotnet run`:

    git clone https://github.com/cristencean/meter-readings.git
	cd EnsekMeterReadings
	dotnet restore
	dotnet build
	cd EnsekMeterReadings.API
	dotnet ef database update
	dotnet run
    testing example: https://localhost:59117/meter-readings
    
## Run the unit testing

To run the unit testing just type `dotnet test`:

    dotnet test

## Project structure

	EnsekMeterReadings/
	│
	├── EnsekMeterReadings.Api/            
	│   └── Controllers/                   # API endpoints
	│
	├── EnsekMeterReadings.Application/    # Services and business logic
	│   ├── Services/                      # Business logic
	│   └── Validation/                    # Custom validation rules
	│
	├── EnsekMeterReadings.Core/           # Interfaces and Models
	│
	├── EnsekMeterReadings.DataAccess/     # EF Core DbContext and Repository
	│
	├── EnsekMeterReadings.Tests/          # xUnit tests with Moq
	│
	├── EnsekMeterReadings.FE/             # React frontend (with Tailwind)
	│   └── src/
	│       ├── components/
	│       │   ├── AddMeterReadings.tsx   # Upload CSV form
	│       │   └── MeterReadingsList.tsx  # Display data
	│       └── utils/constants.ts
	│
	└── README.md