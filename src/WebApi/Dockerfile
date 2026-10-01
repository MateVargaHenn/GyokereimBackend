FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

RUN mkdir -p /app/Models \
    && chown -R $APP_UID:$APP_UID /app/Models

USER $APP_UID

EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /repo

COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]

COPY ["src/WebApi/WebApi.csproj", "src/WebApi/"]
COPY ["src/Modules/Chat/Modules.Chat/Modules.Chat.csproj", "src/Modules/Chat/Modules.Chat/"]
COPY ["src/Common/Common.SharedKernel/Common.SharedKernel.csproj", "src/Common/Common.SharedKernel/"]

RUN dotnet restore "src/WebApi/WebApi.csproj"

COPY . .

WORKDIR "/repo/src/WebApi"

RUN dotnet build "WebApi.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build


FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "WebApi.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WebApi.dll"]