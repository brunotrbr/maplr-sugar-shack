## maplr-api
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src 

COPY . ./
RUN dotnet restore "maplr-api/maplr-api.csproj"

WORKDIR "/src/."
RUN dotnet build "maplr-api/maplr-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "maplr-api/maplr-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "maplr-api.dll"]

## TestMaplrSugarShack
WORKDIR /src
RUN dotnet restore "TestMaplrSugarShack/TestMaplrSugarShack.csproj" 

WORKDIR "/src/."
RUN dotnet build "TestMaplrSugarShack/TestMaplrSugarShack.csproj" -c Release -o /app/build

FROM build AS testrunner
WORKDIR /app/build
CMD ["dotnet", "test", "-e", "ASPNETCORE_ENVIRONMENT=docker"]


