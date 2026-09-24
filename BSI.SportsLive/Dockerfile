FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["BSI.SportsLive.csproj", "./"]
RUN dotnet restore "BSI.SportsLive.csproj"

COPY . .
RUN dotnet publish "BSI.SportsLive.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "BSI.SportsLive.dll"]
