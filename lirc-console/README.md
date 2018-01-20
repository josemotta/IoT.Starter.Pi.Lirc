# Lirc-Console

### Building and pushing to Dockerhub

	$ docker-compose -f lirc-compose.yml build
	Building lirc
	Step 1/24 : FROM microsoft/dotnet:2.0.0-runtime-stretch-arm32v7 AS base
	 ---> 6354e860c381
	Step 2/24 : WORKDIR /app
	 ---> Using cache
	 ---> 05412cd53d5a
	Step 3/24 : ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
	 ---> Using cache
	 ---> 12cf4637e50e
	Step 4/24 : RUN apt-get update   && apt-get upgrade -y   && apt-get install -y --no-install-recommends        lirc   && rm -rf /var/lib/apt/lists/*
	 ---> Using cache
	 ---> 0b28f4942ec9
	Step 5/24 : RUN mkdir -p /var/run/lirc   && rm -f /etc/lirc/lircd.conf.d/devinput.*
	 ---> Using cache
	 ---> 8599c290104e
	Step 6/24 : COPY Lirc/setup/config.txt /boot/config.txt
	 ---> Using cache
	 ---> 1b90462496c0
	Step 7/24 : COPY Lirc/setup/lirc_options.conf /etc/lirc/lirc_options.conf
	 ---> Using cache
	 ---> 4270b263f046
	Step 8/24 : COPY Lirc/setup/ir-remote.conf /etc/modprobe.d/ir-remote.conf
	 ---> Using cache
	 ---> 19ac2e2872ff
	Step 9/24 : COPY Lirc/remotes /etc/lirc/lircd.conf.d
	 ---> Using cache
	 ---> 4a710c8d0f7d
	Step 10/24 : FROM microsoft/dotnet:2.0-sdk AS build
	 ---> 730e3899d926
	Step 11/24 : WORKDIR /src
	 ---> e8abbbf5f6e9
	Removing intermediate container 8a58bc431b60
	Step 12/24 : ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
	 ---> Running in 6def39dc1fa7
	 ---> 9aebdfd73284
	Removing intermediate container 6def39dc1fa7
	Step 13/24 : COPY *.sln .
	 ---> 5e860a31d313
	Step 14/24 : COPY Contest/Contest.csproj Contest/
	 ---> 8a3caeb83910
	Step 15/24 : RUN dotnet restore
	 ---> Running in d0f9d060571c
	/usr/share/dotnet/sdk/2.1.4/NuGet.targets(227,5): warning MSB3202: The project file "/src/docker-compose.dcproj" was not found. [/src/Console.sln]
	/src/docker-compose.dcproj : warning NU1503: Skipping restore for project '/src/docker-compose.dcproj'. The project file may be invalid or missing targets required for restore. [/src/Console.sln]
	  Restoring packages for /src/Contest/Contest.csproj...
	  Generating MSBuild file /src/Contest/obj/Contest.csproj.nuget.g.props.
	  Generating MSBuild file /src/Contest/obj/Contest.csproj.nuget.g.targets.
	  Restore completed in 198.79 ms for /src/Contest/Contest.csproj.
	 ---> 209d1043c558
	Removing intermediate container d0f9d060571c
	Step 16/24 : COPY . .
	 ---> 192e92eeb6c8
	Step 17/24 : WORKDIR /src/Contest
	 ---> 519396d48efb
	Removing intermediate container 331d28acecc3
	Step 18/24 : RUN dotnet build -c Release -r linux-arm -o /app
	 ---> Running in f5f581f9dda0
	Microsoft (R) Build Engine version 15.5.180.51428 for .NET Core
	Copyright (C) Microsoft Corporation. All rights reserved.
	
	  Restoring packages for /src/Contest/Contest.csproj...
	  Installing runtime.linux-arm.Microsoft.NETCore.DotNetAppHost 2.0.0.
	  Installing runtime.linux-arm.Microsoft.NETCore.DotNetHostResolver 2.0.0.
	  Installing runtime.linux-arm.Microsoft.NETCore.DotNetHostPolicy 2.0.0.
	  Installing runtime.linux-arm.Microsoft.NETCore.App 2.0.0.
	  Restore completed in 6.63 sec for /src/Contest/Contest.csproj.
	  Contest -> /app/Contest.dll
	
	Build succeeded.
	    0 Warning(s)
	    0 Error(s)
	
	Time Elapsed 00:00:09.49
	 ---> 5118d530dace
	Removing intermediate container f5f581f9dda0
	Step 19/24 : FROM build AS publish
	 ---> 5118d530dace
	Step 20/24 : RUN dotnet publish -c Release -r linux-arm -o /app
	 ---> Running in 6d95b1cdfa40
	Microsoft (R) Build Engine version 15.5.180.51428 for .NET Core
	Copyright (C) Microsoft Corporation. All rights reserved.
	
	  Restore completed in 24.35 ms for /src/Contest/Contest.csproj.
	  Contest -> /src/Contest/bin/Release/netcoreapp2.0/linux-arm/Contest.dll
	  Contest -> /app/
	 ---> 55409c2731a0
	Removing intermediate container 6d95b1cdfa40
	Step 21/24 : FROM base AS final
	 ---> 4a710c8d0f7d
	Step 22/24 : WORKDIR /app
	 ---> Using cache
	 ---> a2ea12d478d4
	Step 23/24 : COPY --from=publish /app .
	 ---> Using cache
	 ---> 45d03827ff17
	Step 24/24 : ENTRYPOINT dotnet Contest.dll
	 ---> Using cache
	 ---> 4b8ec200357a
	Successfully built 4b8ec200357a
	Successfully tagged josemottalopes/lirconsole:latest
	
	jo@CANOAS24 MINGW64 /c/_git/Lirc-Console/Console (master)
	$ docker push josemottalopes/lirconsole
	The push refers to a repository [docker.io/josemottalopes/lirconsole]
	bdfbceb06300: Mounted from josemottalopes/home-lirc
	a5263a5a63ea: Mounted from josemottalopes/home-lirc
	0308d77027ae: Mounted from josemottalopes/home-lirc
	736189974ffd: Mounted from josemottalopes/home-lirc
	6c15a4e79b6c: Mounted from josemottalopes/home-lirc
	38cd25f9bbe4: Mounted from josemottalopes/home-lirc
	f4dd319f6d5c: Mounted from josemottalopes/home-lirc
	aa89badf53d9: Mounted from josemottalopes/conlirc
	202255094ceb: Mounted from josemottalopes/conlirc
	649673d2d837: Mounted from josemottalopes/conlirc
	643a426f2599: Mounted from josemottalopes/conlirc
	ccd48fa5ba35: Mounted from josemottalopes/conlirc
	latest: digest: sha256:45413fab32528e3649035fb2e1af90c0f8e80272d7b93ae00f8949cff141b454 size: 2829

### Pulling and running at RPI

Below the docker command for the RPI. Please note that it should be equipped with Lirc properly installed and configured.

	docker run -it --name home-lirc --privileged -v /var/run/lirc:/var/run/lirc josemottalopes/lirconsole:latest

The test session below starts with `Hello World!` message. Then, the available remotes are listed by `irsend list "" ""`. You can check that there is a Samsung IR remote control in the list. Last, the `irsend` command is used again, commanding the "volume down" key of Samsung monitor. 

	root@lumi:~# docker run -it --privileged --name lirconsole -v /var/run/lirc:/var/run/lirc josemottalopes/lirconsole:latest
	Unable to find image 'josemottalopes/lirconsole:latest' locally
	latest: Pulling from josemottalopes/lirconsole
	Digest: sha256:45413fab32528e3649035fb2e1af90c0f8e80272d7b93ae00f8949cff141b454
	Status: Downloaded newer image for josemottalopes/lirconsole:latest
	Hello World!
	irsend list "" ""
	irsend list "" ""
	Samsung_BN59-00678A
	LED_24_KEY
	LED_44_KEY
	
	
	irsend send_once Samsung_BN59-00678A KEY_VOLUMEDOWN
	irsend send_once Samsung_BN59-00678A KEY_VOLUMEDOWN
	
	irsend send_once Samsung_BN59-00678A KEY_VOLUMEDOWN
	irsend send_once Samsung_BN59-00678A KEY_VOLUMEDOWN
	
	x
	x
	/bin/bash: x: command not found
	
	root@lumi:~#

As shown at photo below, the monitor acknowledged and accepted the command to decrease the volume.

![](https://i.imgur.com/BVzv3I2.png)

Have fun!

