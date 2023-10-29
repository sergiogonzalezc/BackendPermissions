#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 80
ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BackendPermissions.Api/BackendPermissions.Api.csproj", "BackendPermissions.Api/"]
COPY ["BackendPermissions.Application/BackendPermissions.Application.csproj", "BackendPermissions.Application/"]
COPY ["BackendPermissions.Common/BackendPermissions.Common.csproj", "BackendPermissions.Common/"]
COPY ["BackendPermissions.DataAccess/BackendPermissions.DataAccess.csproj", "BackendPermissions.DataAccess/"]
RUN dotnet restore "BackendPermissions.Api/BackendPermissions.Api.csproj"
COPY . .
WORKDIR "/src/BackendPermissions.Api"
RUN dotnet build "BackendPermissions.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackendPermissions.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendPermissions.Api.dll"]