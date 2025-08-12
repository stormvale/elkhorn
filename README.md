# Project: Elkhorn

Project: Elkhorn is a cloud-native microservices application intended to ease interactions between
schools and families. Built to serve as a learning/experimentation platform for several techniques
and technologies, including Dapr, .NET Aspire, EF Core for Azure Cosmos DB, React, Redux Toolkit,
TestContainers, Infrastructure as Code (IaC) using Terraform, Api Gateway using Yarp, CI/CD using
GitHub Actions, Authentication (OIDC via MSAL, Auth2), multi-tenancy and other stuff.

## Structure

- **src/**: Main source code
  - **AppHost/**: .NET Aspire host for running locally
  - **services/**: API microservices
  - **Domain/**: Shared domain classes and abstractions
  - **Contracts/**: Shared API contracts & Dapr message contracts
  - **clients/**: Web clients (UI)
- **test/**: Tests
  - **unit/**: Unit tests. Should be one test project per microservice
  - **integration/**: Integration tests (TestContainers)
  - **architecture/**: *coming soon*

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js & npm](https://nodejs.org/)
- [Dapr CLI](https://docs.dapr.io/get-dapr/)
- [Docker Desktop](https://docs.docker.com/desktop/setup/install/windows-install/)

### Running the application locally

1. **Run the docker-compose file (for local stmp email service)**
   ```sh
   docker compose up -d
   ```

2. **Have Dapr initialized**
   ```sh
   dapr init
   ```
   (verify in Docker Desktop that the `dapr_*` containers are running.)


3. **Run AppHost**
   ```sh
   dotnet run --project src/AppHost/AppHost.csproj
   ```

4. **Navigate to the Aspire Dashboard**
    - The OpenApi documentation can be viewed using the 'Scalar API Reference' URL. (also available for each service at `/openapi/v1.json`)
    - CosmodDb Data Explorer can be accessed using the 'Data Explorer' URL.
    - The React clients can be launched using the URL.

---