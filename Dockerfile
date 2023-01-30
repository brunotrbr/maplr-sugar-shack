## maplr-api
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-api
WORKDIR /src 

COPY . ./
RUN dotnet restore "maplr-api/maplr-api.csproj"

WORKDIR "/src/."
RUN dotnet build "maplr-api/maplr-api.csproj" -c Release -o /app/build

FROM build-api AS publish
RUN dotnet publish "maplr-api/maplr-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "maplr-api.dll"]

## test-maplr-api
FROM build-api as build-tests
WORKDIR /src

RUN dotnet restore "test-maplr-api/test-maplr-api.csproj" 

WORKDIR "/src/."
RUN dotnet build "test-maplr-api/test-maplr-api.csproj" -c Release -o /app/build

FROM build-tests AS testrunner
WORKDIR /app/build
CMD ["dotnet", "test", "-e", "ASPNETCORE_ENVIRONMENT=docker"]
