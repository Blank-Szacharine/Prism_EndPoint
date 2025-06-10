FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5257
EXPOSE 5257

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY . .
WORKDIR "/src/Prism_EndPoint"
RUN dotnet build "Prism_EndPoint.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Prism_EndPoint.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-self-contained


FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","Prism_EndPoint.dll"]
