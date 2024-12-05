# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5076

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HotelService.Api/HotelService.Api.csproj", "HotelService.Api/"]
COPY ["App.Core/App.Core.csproj", "App.Core/"]
COPY ["HotelService.Application/HotelService.Application.csproj", "HotelService.Application/"]
COPY ["HotelService.Data/HotelService.Data.csproj", "HotelService.Data/"]
RUN dotnet restore "HotelService.Api/HotelService.Api.csproj"
COPY . .
WORKDIR "/src/HotelService.Api"
RUN dotnet build "HotelService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HotelService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelService.Api.dll"]
