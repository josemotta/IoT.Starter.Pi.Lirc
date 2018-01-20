FROM microsoft/dotnet:2.0.0-runtime-stretch-arm32v7 AS base
WORKDIR /app
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY *.sln .
COPY Contest/Contest.csproj Contest/
RUN dotnet restore
COPY . .
WORKDIR /src/Contest
RUN dotnet build -c Release -r linux-arm -o /app

FROM build AS publish
RUN dotnet publish -c Release -r linux-arm -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Contest.dll"]