# ApiRest - OpenCode Agent Guidance

## Project Structure
- Single ASP.NET Core Web API project targeting .NET 10.0
- Root solution file: `ApiRest.slnx`
- Main project: `ApiRest/ApiRest.csproj`
- Default controllers location: `ApiRest/Controllers/`

## Key Commands
```bash
# Build and run the application
dotnet build
dotnet run

# Run with specific profile (from launchSettings.json)
dotnet run --launch-profile https  # Uses https://localhost:7221
dotnet run --launch-profile http   # Uses http://localhost:5038

# Restore packages
dotnet restore
```

## Development Configuration
- OpenAPI is enabled in development mode (`app.MapOpenApi()` in Program.cs)
- Development environment uses HTTPS on port 7221, HTTP on port 5038
- No existing test projects or test configuration found
- Minimal dependencies: only `Microsoft.AspNetCore.OpenApi`

## Important Notes
- No linting or formatting tools configured in project
- No CI/CD workflows present
- Project uses nullable reference types (`<Nullable>enable</Nullable>`)
- Implicit usings enabled (`<ImplicitUsings>enable</ImplicitUsings>`)
- Generated directories (bin/, obj/, .vs/) are ignored via .gitignore

## Setup
1. Ensure .NET 10.0 SDK is installed
2. Run `dotnet dev-certs https --trust` to trust development HTTPS certificate
3. Restore packages: `dotnet restore`
4. Build: `dotnet build`
5. Run: `dotnet run` or use VS Code/Visual Studio launch configurations