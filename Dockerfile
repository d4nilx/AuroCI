FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY AuroCI.sln .
COPY AuroCI.Core/AuroCI.Core.csproj AuroCI.Core/  
COPY AuroCI.CLI/AuroCI.CLI.csproj AuroCI.CLI/ 

RUN dotnet restore 

COPY AuroCI.CLI/ AuroCI.CLI/
COPY AuroCI.Core/ AuroCI.Core/
RUN dotnet publish AuroCI.CLI/AuroCI.CLI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AuroCI.CLI.dll"]