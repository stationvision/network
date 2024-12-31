#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

COPY ["src/NetworkCommunication/NetworkCommunications.csproj", "NetworkCommunications.csproj"]
COPY ["src/Monitoring.Core/Monitoring.Core.csproj", "Monitoring.Core.csproj"]
COPY ["src/Monitoring.Db/Monitoring.Db.csproj", "Monitoring.Db.csproj"]
RUN dotnet restore "NetworkCommunications.csproj"
COPY . .
WORKDIR /src/NetworkCommunication
RUN dotnet build "NetworkCommunications.csproj" -c Release -o /app/publish

FROM build AS publish
WORKDIR /src/NetworkCommunication
RUN dotnet publish "NetworkCommunications.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetworkCommunications.dll"]
COPY ./cert.pfx /app/https.pfx
ENV ASPNETCORE_URLS="http://+:80"
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="www321"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/app/https.pfx"