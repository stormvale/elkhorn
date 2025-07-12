# Project: Elkhorn

Project: Elkhorn is a distributed, cloud-native application intended to ease interactions between schools and families. Built to serve as a learning/experimentation platform

for managing restaurants, schools, lunches, and orders, built with .NET, Dapr, Azure Cosmos DB, and React.

## Structure

- **src/**: Main source code
  - **AppHost/**: .NET Aspire host for running locally
  - **services/**: API microservices (Restaurants, Lunches, Notifications, etc.)
  - **Domain/**: Shared domain classes and abstractions
  - **Contracts/**:  Shared API contracts & Dapr message contracts
  - **clients/web-react-ts-mui/**: React + TypeScript web client

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js & npm](https://nodejs.org/)
- [Dapr CLI](https://docs.dapr.io/get-dapr/)
- [Docker Desktop]((https://docs.docker.com/desktop/setup/install/windows-install/))

### Running the Application

1. **Run the docker-compose file (for local stmp email service)**
   ```sh
   docker compose up -d
   ```

2. **Have Dapr initialized**
   ```sh
   dapr init
   ```
   (`dapr_*` containers should be running.)

3. **Run AppHost**
   ```sh
   dotnet run --project src/AppHost/AppHost.csproj
   ```

4. **Launch the Aspire Dashboard**
    - The OpenApi documentation can be viewed using the 'Scalar API Reference' URL. (also available for each service at `/openapi/v1.json`)
    - CosmodDb Data Explorer can be accessed using the 'Data Explorer' URL.
    - The React client can be launched using the URL for the 'web-react-ts-mui' resource.

---