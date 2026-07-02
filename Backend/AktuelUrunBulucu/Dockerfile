FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY AktuelUrunBulucu.csproj .
RUN dotnet restore AktuelUrunBulucu.csproj

COPY . .
RUN dotnet publish AktuelUrunBulucu.csproj --configuration Release --output /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5012
EXPOSE 5012

ENTRYPOINT ["dotnet", "AktuelUrunBulucu.dll"]
