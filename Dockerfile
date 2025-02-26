FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY . .
RUN dotnet restore

WORKDIR /app/src/PPMRm.Web
RUN dotnet publish -c release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "PPMRm.Web.dll"]
