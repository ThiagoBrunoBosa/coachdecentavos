FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY CoachDecentavos.slnx ./
COPY src/CoachDecentavos.Domain/CoachDecentavos.Domain.csproj src/CoachDecentavos.Domain/
COPY src/CoachDecentavos.Application/CoachDecentavos.Application.csproj src/CoachDecentavos.Application/
COPY src/CoachDecentavos.Infrastructure/CoachDecentavos.Infrastructure.csproj src/CoachDecentavos.Infrastructure/
COPY src/CoachDecentavos.Api/CoachDecentavos.Api.csproj src/CoachDecentavos.Api/
RUN dotnet restore src/CoachDecentavos.Api/CoachDecentavos.Api.csproj
COPY src/ src/
WORKDIR /src/src/CoachDecentavos.Api
RUN dotnet publish CoachDecentavos.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
RUN apt-get update && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CoachDecentavos.Api.dll"]
