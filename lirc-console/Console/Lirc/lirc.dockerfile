FROM microsoft/dotnet:2.0.0-runtime-stretch-arm32v7 AS base
WORKDIR /app
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

RUN \
  apt-get update \
  && apt-get upgrade -y \
  && apt-get install -y \
       lirc \
  --no-install-recommends && \
  rm -rf /var/lib/apt/lists/*

RUN \
  mkdir -p /var/run/lirc \
  && rm -f /etc/lirc/lircd.conf.d/devinput.*

COPY Lirc/setup/config.txt /boot/config.txt
COPY Lirc/setup/lirc_options.conf /etc/lirc/lirc_options.conf 
COPY Lirc/setup/ir-remote.conf /etc/modprobe.d/ir-remote.conf
COPY Lirc/remotes /etc/lirc/lircd.conf.d

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
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
