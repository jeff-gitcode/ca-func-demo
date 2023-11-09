# Func DEMO

## Tech Stack

-[x] Azure Function

```c#
$ func init func-demo --worker-runtime dotnet-isolated

$ cd func-demo

$ func new --template "Http Trigger" --name HttpTrigger-Demo

$ func new --template "Azure Queue Storage Trigger" --name QueueTrigger-Demo

$ func start

$ curl --get http://localhost:7071/api/HttpTrigger_Demo?name=JeffTest

$ curl --request POST http://localhost:7071/api/MyHttpTrigger --data '{"name":"Jeff Test"}'

```

## Debug

![Alt text](azure-func-debug-demo.gif)


# CA Azure Function Demo

## Tech Stack
- [x] Azure Function
- [x] CA
- 
- 
- 

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
$ func start
$ curl --get http://localhost:7071/api/HttpTrigger_Demo?name=JeffTest

```