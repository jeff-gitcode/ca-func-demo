# CA Azure Function Demo

## Tech Stack
- [x] Azure Function
- [x] CA
- [x] FluentValidation
- [x] AutoMapper
- [x] OpenApi
- [x] Newtonsoft
- [x] Middleware

```dotnetcli

$ mkdir ca-func-demo
$ cd ca-func-demo
$ dotnet new sln --name ca-func-demo

$ func init Function --worker-runtime dotnet-isolated

$ dotnet new classlib -o Domain
$ dotnet new classlib -o Application
$ dotnet new classlib -o Infrastructure
$ dotnet new classlib -o Presentation

$ dotnet add .\Application\ reference .\Domain\
$ dotnet add .\Infrastructure\ reference .\Application\
$ dotnet add .\Presentation\ reference .\Application\

$ dotnet sln add (ls -r **//*.csproj)

$ cd Function
$ func new --template "Http Trigger" --name HttpTrigger-Demo
$ func new --name SearchUserFunction --template "HTTP trigger" --authlevel "anonymous"
$ func start
$ curl --get http://localhost:7071/api/HttpTrigger_Demo?name=JeffTest

# cosmo db emulator Download the Azure Cosmos DB emulator

$ cd %ProgramFiles%\Azure Cosmos DB Emulator
$ Microsoft.Azure.Cosmos.Emulator.exe /Port=8081

https://localhost:8081/_explorer/index.html


```