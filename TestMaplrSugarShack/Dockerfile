FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src 

COPY ["TestMaplrSugarShack.csproj", "./"] 
RUN dotnet restore "TestMaplrSugarShack.csproj" 
COPY . . 
WORKDIR "/src/."
RUN dotnet build "TestMaplrSugarShack.csproj" -c Release -o /app/build

FROM build AS testrunner
WORKDIR /app/build
CMD ["dotnet", "test", "-e", "ASPNETCORE_ENVIRONMENT=docker"]