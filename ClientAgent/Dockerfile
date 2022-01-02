#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ClientAgent.csproj", "."]
COPY ["../Inventory.Common/Inventory.Common.csproj", "../Inventory.Common/"]
RUN dotnet restore "./ClientAgent.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ClientAgent.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClientAgent.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV DOCKER_RUNNING=true
ENTRYPOINT ["dotnet", "ClientAgent.dll"]