#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM base AS final
WORKDIR /app
COPY ./publish/ .
ENTRYPOINT ["dotnet", "2DSupplyStation.dll"]