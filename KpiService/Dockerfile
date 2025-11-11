FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["KpiService.csproj", "./"]
RUN dotnet restore "KpiService.csproj"

# copy everything and publish
COPY . .
RUN dotnet publish "KpiService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

# Recommended default environment; override on Render or with docker run
ENV DOTNET_ENVIRONMENT=Production

ENTRYPOINT [ "dotnet", "KpiService.dll" ]
