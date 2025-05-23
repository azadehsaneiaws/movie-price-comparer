#  Movie Price Comparer

Compare movie prices between **Cinemaworld** and **Filmworld** using this full-stack web app built with **.NET 8** backend and **React.js** frontend.

---

##  Features

###  Backend (.NET 8)
- **Multiple Providers**: Fetches data from two external APIs (Cinemaworld & Filmworld).
- **Retry Policy**: Implements automatic retry (3 attempts, 500ms delay) using Polly to handle transient API failures.
- **Caching**: Uses `IMemoryCache` to cache movie list for 10 minutes and movie detail comparison for 5 minutes to reduce load.
- **Fallback Handling**: Returns partial data if one provider fails (never blocks whole result).
- **Error Logging**: Centralized logging for API call failures and exceptions.
- **Dependency Injection**: Clean and testable architecture using interfaces and providers.
- **Swagger UI**: Integrated Swagger at `/swagger` for testing and exploring API.

###  Frontend (React.js + Axios + Bootstrap)
- **Movie List View**: Fetches and displays movies from the backend with title, year, provider, and cheapest price.
- **Search Functionality**: Live filter to search movies by title.
- **Error Messaging**: Friendly message if API call fails or no movies are returned.
- **Responsive UI**: Clean and responsive layout using Bootstrap 5.
- **Environment-based Config**: API URL configured via `.env` file.

---

##  Technologies Used

| Layer        | Stack                         |
|--------------|-------------------------------|
| **Frontend** | React.js, Axios,Material UI   |
| **Backend**  | .NET 8, Polly, MemoryCache    |

---

## 🧪 Error Handling & Resilience

- **Try/Catch** blocks on all controller and repository methods
- **Retry Logic**: Automatically retries failed API requests
- **Fallback Logic**: Skips failed providers and uses available data
- **Caching**: Prevents repeated calls and speeds up response
- **Friendly UI Errors**: "Failed to load movies" message shown on frontend

---

##  Getting Started

### 1. Backend (.NET API)

1. Restore NuGet packages:
   dotnet restore
2. Set up your appsettings.json:

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ApiSettings": {
    "Token": "your-api-token-here"
  }
}
Run the API:


dotnet run --project MoviePriceComparer.API
Navigate to:

https://localhost:7088/swagger
### 2. Frontend (React)
Create a .env file in moviepricecomparer.frontend folder:


REACT_APP_API_URL=http://localhost:7088/api
Install dependencies:

npm install
Run the frontend:
npm start
Open in browser:
http://localhost:3000

## Author Notes
This is a full-stack demo project showcasing:

Clean architecture with DI and interfaces

Real-world API consumption with fault tolerance

Elegant React UI with meaningful error boundaries

Great for interviews or as a GitHub portfolio project

