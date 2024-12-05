FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5078

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ReportService.Api/ReportService.Api.csproj", "ReportService.Api/"]
COPY ["ReportService.Data/ReportService.Data.csproj", "ReportService.Data/"]
COPY ["App.Core/App.Core.csproj", "App.Core/"]
COPY ["ReportService.Application/ReportService.Application.csproj", "ReportService.Application/"]
COPY ["ReportService.Consumer/ReportService.Consumer.csproj", "ReportService.Consumer/"]
COPY ["ReportService.Client/ReportService.Client.csproj", "ReportService.Client/"]
RUN dotnet restore "ReportService.Api/ReportService.Api.csproj"
COPY . .
WORKDIR "/src/ReportService.Api"
RUN dotnet build "ReportService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ReportService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReportService.Api.dll"]
