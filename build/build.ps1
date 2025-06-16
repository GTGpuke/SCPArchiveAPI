# Script PowerShell pour automatiser les tâches courantes

param(
    [Parameter(Position=0)]
    [string]$Task = "help"
)

function Show-Help {
    Write-Host @"
Usage: .\build.ps1 <task>

Available tasks:
  build       - Build the solution
  test        - Run all tests
  test:unit   - Run unit tests only
  test:int    - Run integration tests only
  test:e2e    - Run end-to-end tests only
  clean       - Clean build artifacts
  docker      - Build Docker image
  docker:up   - Start all Docker containers
  docker:down - Stop all Docker containers
  help        - Show this help message
"@
}

function Invoke-Build {
    dotnet build
}

function Invoke-Test {
    dotnet test
}

function Invoke-UnitTest {
    dotnet test --filter "Category=Unit"
}

function Invoke-IntegrationTest {
    dotnet test --filter "Category=Integration"
}

function Invoke-E2ETest {
    dotnet test --filter "Category=E2E"
}

function Invoke-Clean {
    dotnet clean
    Get-ChildItem -Include bin,obj -Recurse | Remove-Item -Recurse -Force
}

function Invoke-DockerBuild {
    docker build -t scparchive-api .
}

function Invoke-DockerUp {
    docker-compose up -d
}

function Invoke-DockerDown {
    docker-compose down
}

switch ($Task.ToLower()) {
    "build" { Invoke-Build }
    "test" { Invoke-Test }
    "test:unit" { Invoke-UnitTest }
    "test:int" { Invoke-IntegrationTest }
    "test:e2e" { Invoke-E2ETest }
    "clean" { Invoke-Clean }
    "docker" { Invoke-DockerBuild }
    "docker:up" { Invoke-DockerUp }
    "docker:down" { Invoke-DockerDown }
    default { Show-Help }
}
