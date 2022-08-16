#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["AzureIoTWeb/AzureIoTWeb.csproj", "AzureIoTWeb/"]
#RUN dotnet restore "AzureIoTWeb/AzureIoTWeb.csproj"
#COPY . .
#WORKDIR "/src/AzureIoTWeb"
#RUN dotnet build "AzureIoTWeb.csproj" -c Release -o /app/build
#
#
#FROM node:16.16.0-alpine AS build-client
#WORKDIR /app
#COPY ../clientapp/package.json ../clientapp/package-lock.json ./
#RUN npm install
#COPY ../clientapp .
#RUN npm run build
#
#FROM build AS publish
#RUN dotnet publish "AzureIoTWeb.csproj" -c Release -o /app/publish
#
#
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#copy --from=build/client /app/publish .
#ENTRYPOINT ["dotnet", "AzureIoTWeb.dll"]
#
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app


COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out



FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
COPY ./clientapp/build ./clientapp/build
ENTRYPOINT [ "dotnet", "AzureIoTWeb.dll" ]