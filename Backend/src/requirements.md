dotnet new webapi -n src

# Install packages
cd src
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.0


# view swagger
http://localhost:5185/swagger/index.html


# For unit test
cd .. (from src)
dotnet new xunit -n src.Tests
// Add reference
dotnet add src.Tests reference src/src.csproj

# PAckages for test
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 9.0.0
dotnet add package coverlet.collector --version 8.0.0
dotnet add package FluentAssertions --version 6.12.0
dotnet add package Microsoft.NET.Test.Sdk --version 17.10.0
dotnet add package xunit --version 2.6.6
dotnet add package xunit.runner.visualstudio --version 2.5.6


# Get tests
dotnet test --collect:"XPlat Code Coverage" `
&& reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coveragereport -reporttypes:Html

dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:coverage.opencover.xml -targetdir:coverage-report -reporttypes:Html

# For test 
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# cd src.test to install
dotnet add package coverlet.collector

# global
dotnet tool install -g dotnet-reportgenerator-globaltool

# inside the src.test
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=coverage.xml

# run in backend folder level
reportgenerator -reports:src.Tests/TestResults/*/coverage.cobertura.xml -targetdir:coverage/html -reporttypes:Html
start coverage/html/index.html

# collect latest
dotnet test src.Tests/src.Tests.csproj `
    --collect:"XPlat Code Coverage" `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput=TestResults/