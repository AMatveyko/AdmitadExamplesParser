#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TheStore.Api.Front/TheStore.Api.Front.csproj", "TheStore.Api.Front/"]
COPY ["TheStore.Api.Front.Data/TheStore.Api.Front.Data.csproj", "TheStore.Api.Front.Data/"]
COPY ["Web.Common/Web.Common.csproj", "Web.Common/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "TheStore.Api.Front/TheStore.Api.Front.csproj"
COPY . .
WORKDIR "/src/TheStore.Api.Front"
RUN dotnet build "TheStore.Api.Front.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TheStore.Api.Front.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheStore.Api.Front.dll"]