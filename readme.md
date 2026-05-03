# Catastrophe Modeling App

## Purpose

Use the NOAA Best Track Data (HURDAT2) to identify all hurricanes that have made landfall in Florida since 1900. Using a programming language of your choice, build an application to parse the HURDAT2 data, identify the hurricanes that made landfall in Florida, and output a report listing the name, date of landfall, and maximum wind speed for each event.

This solution provides:

- A C# ASP.NET Core Web API that processes an uploaded HURDAT2 data file.
- A C# ASP.NET Core MVC front end that lets a user upload the file and view the results in the browser.
- A report showing the storm name, date of landfall, and maximum wind speed for each Florida landfall event found in the dataset.

Per the original assignment, the landfall indicator `L` in the HURDAT2 file should not be used as the determining factor for Florida landfall. The NOAA HURDAT2 dataset and format specification are available here:

https://www.nhc.noaa.gov/data/

## Author

- Author: Mr. Sam T. Leal
- Date: 04/27/2026

## Project Structure

- `catastrophe-modeling-app-api` - ASP.NET Core Web API for HURDAT2 processing.
- `catastrophe-modeling-app-mvc` - ASP.NET Core MVC web application for file upload and results display.

## Prerequisites

Before running the solution, make sure you have:

- .NET 10 SDK installed.
- A local development HTTPS certificate trusted on your machine.
- A HURDAT2 file downloaded from the NOAA website.

If you need to trust the local development certificate, run:

```powershell
dotnet dev-certs https --trust
```

## Run The Project

Open two terminals in the repository root.

### 1. Start the C# API

Run the API project first:

```powershell
dotnet run --project .\catastrophe-modeling-app-api\catastrophe-modeling-app-api.csproj
```

The API runs on:

- `https://localhost:7065`
- `http://localhost:5139`

### 2. Start the MVC Application

In a second terminal, run the MVC project:

```powershell
dotnet run --project .\catastrophe-modeling-app-mvc\catastrophe-modeling-app-mvc.csproj
```

The MVC application runs on:

- `https://localhost:7032`
- `http://localhost:5266`

Open the MVC site in your browser:

- `https://localhost:7032`

## Sample Data File

The HURDAT2 dataset is included in the repository at `./Data/hurdat2-1851-2025-02272026.txt`.

After starting the API project and then the MVC project, click the `Upload` button on the home page, select this file, and submit to view the Florida landfall results.

## Use The Application

After both applications are running:

1. Start the C# API.
2. Start the MVC application.
3. In the MVC browser page, upload a HURDAT2 file using the file upload input.
4. Click `Submit` to begin processing.
5. Wait a moment for the results to display on the screen.

When processing completes, the page displays a report table containing:

- Name of storm
- Date of landfall
- Maximum wind speed achieved

## Notes

- The MVC app submits the uploaded file to the API endpoint at `https://localhost:7065/api/Process/ProcessHURDAT2`.
- The API must be running before you click `Submit`, or the upload request will fail.
- The results table supports paging in the browser after the data is returned.
