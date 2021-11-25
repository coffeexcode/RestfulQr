#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN ln -s /lib/x86_64-linux-gnu/libdl.so.2 /lib/x86_64-linux-gnu/libdl.so
RUN apt update
RUN apt install -y libgdiplus
RUN ln -s /usr/lib/libgdiplus.so /lib/x86_64-linux-gnu/libgdiplus.so
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RestfulQr.Api/RestfulQr.Api.csproj", "RestfulQr.Api/"]
COPY ["RestfulQr.Domain/RestfulQr.Domain.csproj", "RestfulQr.Domain/"]
COPY ["RestfulQr.Persistence/RestfulQr.Persistence.csproj", "RestfulQr.Persistence/"]
COPY ["RestfulQr.Core/RestfulQr.Core.csproj", "RestfulQr.Core/"]
COPY ["RestfulQr.Migrations/RestfulQr.Migrations.csproj", "RestfulQr.Migrations/"]
RUN dotnet restore "RestfulQr.Api/RestfulQr.Api.csproj"
COPY . .
WORKDIR "/src/RestfulQr.Api"
RUN dotnet build "RestfulQr.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestfulQr.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestfulQr.Api.dll"]