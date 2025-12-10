# Clean Architecture
This is the comprehensive version of my Clean Architecture solution. This includes all the layers, projects, and configurations needed to implement a full Clean Architecture pattern in a .NET application. This solution is designed to be a starting point for developers looking to build applications following Clean Architecture principles.

If you found an issue or want to get know more, please ping me here or on my linkedIn account: https://www.linkedin.com/in/mark-joseph-tigno/

## How to get the project running:
Build the solution by executing the following command commands:
- ```dotnet clean```
- ```dotnet restore```
- ```dotnet build```

To migrate the database:
- ```dotnet ef database update --project .\src\Infrastructure\ --startup-project .\src\WebApi\```

To run the project:
- ```dotnet run --project .\src\WebApi\```