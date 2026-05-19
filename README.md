# Clean Architecture
This is the comprehensive version of my Clean Architecture solution. This includes all the layers, projects, and configurations needed to implement a full Clean Architecture pattern in a .NET application. This solution is designed to be a starting point for developers looking to build applications following Clean Architecture principles.

If you found an issue or want to get know more, please ping me here or on my linkedIn account: https://www.linkedin.com/in/mark-joseph-tigno/

## How to get the project running:
Build the solution by executing the following commands on terminal/CLI:
- ```dotnet clean```
- ```dotnet restore```
- ```dotnet build```

Run docker compose to up the database service:
- ```docker componse up -d```

To migrate the database:
- ```dotnet ef database update --project ./src/Infrastructure --startup-project ./src/WebApi```

Optional if you want to add your own migration, execute this first:
- ```dotnet ef migrations add Initial --project ./src/Infrastructure --startup-project ./src/WebApi```

Register a dev certificate on your local machine:
- ```dotnet dev-certs https --clean```
- ```dotnet dev-certs https --trust```

To run the project:
- ```dotnet run --project ./src/WebApi --launch-profile https```

Then open Swagger UI to test the REST API endpoints: https://localhost:5001/swagger/


Note: You can replace what Database system that you will use for this solution. Please configure the `appsettings.json` and comment/uncomment the corresponding code in `Infrastructure/DependencyInjection.cs` to use the database system of your choice. The default is set to SQL Server.