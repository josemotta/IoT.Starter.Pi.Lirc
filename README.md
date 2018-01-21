# IoT.Starter.Pi.Lirc

#### IoT.Starter.Pi.Thing powered by Linux Infrared Remote Control

## Introduction

This series, started by [IoT.Starter.Pi.Core](https://github.com/josemotta/IoT.Starter.Pi.Core), introduced in the second part the [IoT.Starter.Pi.Thing](https://github.com/josemotta/IoT.Starter.Pi.Thing), designed as a standard starter kit for IoT Home automation initiatives.

Based on facts below:

- We´d be able to extract most from the RPI if we better explore the vast and brilliant resources already available for Linux.
- Most home automation initiatives should handle legacy infrared (IR) controlled gadgets we have at home, including sound, image, air, heater, etc.
- Linux Lirc is a mature and wide project. There is a big remote database containing config files for remote controls. More than 2,500 devices and counting.
- IoT.Starter.Pi.Thing can benefit from Lirc installed at RPI, we just need to link them.

The specification for current mission is to provide infrared (IR) capability to `IoT.Starter.Pi.Thing` projects. 

## LIRC: Linux Infrared Remote Control for Raspberry Pi

The Linux Infrared Remote Control for Raspberry Pi is derived from the original Lirc serial driver by [Aron Szabo](http://aron.ws/projects/lirc_rpi/ "original lirc for rpi"). A further development by Bengt Martensson [improved the Lirc driver](https://github.com/bengtmartensson/lirc_rpi "lirc_rpi"). The good news, for you installing Lirc for the first time, is that Raspberry Pi / Raspbian Stretch comes with Lircd (version 0.9.4c here), improved  to make your life better. Files required at previous version are not used anymore: `/etc/modules` and `/etc/lirc/hardware.conf`.  

### Lirc install

In order to update and upgrade Raspbian and install Lirc at RPI, run the command:

	  apt-get update \
	  && apt-get upgrade -y \
	  && apt-get install -y lirc \
	  && rm -rf /var/lib/apt/lists/*

### Config.txt

The Lirc version 0.9.4c is configured ONLY by file `/etc/config.txt`.

Add to /boot/config.txt:

    # Uncomment this to enable the lirc-rpi module
    dtoverlay=lirc-rpi,gpio_out_pin=17,gpio_in_pin=18,gpio_in_pull=up

### Change default driver

Edit file /etc/lirc/lirc_options.conf and change:

    from:
    driver  = devinput
    device  = auto
    
    to:
    driver  = default
    device  = /dev/lirc0
    
## Reboot and check

Please check more details about Lirc installation at [RPI Setup instructions](https://github.com/josemotta/IoT.Starter.Pi.Lirc/blob/master/RPI_Setup.md) available in the project repo. Following is a quick check to evaluate Lirc installation.

#### lircd status

	/etc/init.d/lircd status
	[ ok ] lircd is running.

#### lsmod

    lsmod | grep lirc
	lirc_rpi                9032  0
	lirc_dev               10583  1 lirc_rpi
	rc_core                24377  1 lirc_dev

#### devices

    mode2 --driver default --list-devices
    /dev/lirc0

#### Checking available remotes and their respective IR codes

	pi@lumi:~ $ irsend list "" ""

	LED_24_KEY
	LED_44_KEY
	pi@lumi:~ $ irsend list LED_24_KEY ""

	0000000000000001 BRIGHT_DOWN
	0000000000000002 BRIGHT_UP
	0000000000000003 OFF
	0000000000000004 ON
	0000000000000005 RED
	0000000000000006 GREEN
	0000000000000007 BLUE
	0000000000000008 WHITE
	0000000000000009 ORANGE
	000000000000000a PEA_GREEN
	000000000000000b DARK_BLUE
	000000000000000c 7_JUMP
	000000000000000d DARK_YELLOW
	000000000000000e CYAN
	000000000000000f BROWN
	0000000000000010 ALL_FADE
	0000000000000011 YELLOW
	0000000000000012 LIGHT_BLUE
	0000000000000013 PINK
	0000000000000014 7_FADE
	0000000000000015 STRAW_YELLOW
	0000000000000016 SKY_BLUE
	0000000000000017 PURPLE
	0000000000000018 3_JUMP

#### Blasting IR commands to RGB lights

	# turn on
	pi@lumi:~ $ irsend SEND_ONCE LED_44_KEY POWER
	#change color
	pi@lumi:~ $ irsend SEND_ONCE LED_44_KEY WHITE
	pi@lumi:~ $ irsend SEND_ONCE LED_44_KEY CYAN
	pi@lumi:~ $ irsend SEND_ONCE LED_44_KEY WHITE
	# lights up and down
	pi@lumi:~ $ irsend --count=10 SEND_ONCE LED_44_KEY BRIGHT_UP
	pi@lumi:~ $ irsend --count=10 SEND_ONCE LED_44_KEY BRIGHT_UP
	pi@lumi:~ $ irsend --count=20 SEND_ONCE LED_44_KEY BRIGHT_DOWN
	pi@lumi:~ $ irsend --count=10 SEND_ONCE LED_44_KEY BRIGHT_UP
	# turn off
	pi@lumi:~ $ irsend SEND_ONCE LED_44_KEY POWER

## Lirc-Console

Lirc-Console extends IoT.Starter.Pi.Thing projects to use Linux Infrared Remote Control.

Please note that, until now, `home-ui` and `home-web` projects were built with no knowledge about Lirc. The `lirconsole` objective is to start Lirc commands from a docker container, and communicate with Lirc installed at RPI host. As we will see, the `irsend` command will play an important role here, identifying remotes, their corresponding codes, and blasting IR led streams to control home equipment. 

Console program is a simple loop, as shown below, that echoes commands. 

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string example = "";

            while (!example.ToLower().Equals("x"))
            {
                example = Console.ReadLine();
                Console.WriteLine(example);
                Console.WriteLine(example.Bash());
            }

            //Console.Read();
        }
    }

The ShellHelper class suggested by [Loune.net](https://loune.net/2017/06/running-shell-bash-commands-in-net-core/) does the dirty job, starting a bash process,  capturing the answer, and returning the `string` result.

    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }

Well done. Actually, we now reached the real job at this mission.

## Lirc on docker container

Based on the original [Lirc basic setup flow](http://www.lirc.org/html/configuration-guide.html#basic-setup-flow), the edited diagram below shows Lirc installed at RPI host. It includes all low level devices, drivers, `lircd` socket and `lircd.conf` configuration files, they are all installed at RPI host, near hardware level.

	
	      ------------
	      |  remote  |
	      ------------
	
	        (air gap)
	
	      ------------
	      ! capture  !
	      ! device   !
	      ------------
	           |
	           v
	           |
	      ------------
	      ! kernel   !                   Sometimes needs
	      ! driver   !                   modprobe(1) configuration.
	      ------------
	           |
	           v  IR pulse data          Device like /dev/lirc0, /dev/ttyACM0.
	           |                         or /dev/ttyS0.
	      ------------
	      |  lirc    |                   Configure lirc_options.conf
	      |  driver  |                   with driver and usually also device.
	      ------------
	           |
	           v  IR pulse data          Use mode2(1) to debug
	           |
	   ----------------
	   |  lirc pass 1 |                  lircd.conf config file.
	   ----------------
	           |
	           v  Key symbols            Output socket e. g.,
	           |                         /var/run/lirc/lircd. Use irw(1) to debug.
	HOST
	===============================================================================
	CONTAINER
	           |
	   ----------------
	   |  lirconsole  |                  lirconsole app.
	   ----------------


The `lirconsole` program that is running in the container communicates exactly through the `lircd` socket. This establishes "the link" using the  docker `volume` concept. Please see below the `Lirc-compose.yml` and `Lirc.dockerfile` files that do all job.

#### Lirc-compose.yml

The volume is created exactly at socket area: `/var/run/lirc`. This insures the proper communication between containers running `irsend` commands and Lirc output socket installed at RPI host.

	version: '3'
	
	services:
	  lirc:
	    container_name: lirconsole
	    image: josemottalopes/lirconsole
	    build:
	      context: .
	      dockerfile: Lirc/lirc.Dockerfile
	    network_mode: bridge
	    privileged: true
	    volumes:
	    - /var/run/lirc:/var/run/lirc
	    environment:
	      - ASPNETCORE_ENVIRONMENT=Development

#### Lirc.dockerfile

As usual, the `dotnet:2.0.0-runtime-stretch-arm32v7` image is used as `base` and the OS is updated and upgraded before installing Lirc. After Lirc package installation, the RPI configuration is copied to Lirc inside container. This assures that remotes that are installed at RPI host be seen by docker container software. The `/boot/config.txt` and built in remotes are moved to /`etc/lirc/lircd.conf.d`, keeping the same setup installed at RPI host.

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

Finally, the `Contest` console program is built and copied to the `base` image. The entry point is set exactly at the console program. It is now time to build and push the image to DockerHub.

### Building and pushing to Dockerhub

You can notice that several dockerfile and docker-compose files were kept in the solution. To build the `lirconsole` properly, the `lirc-compose.yml` and `lirc.Dockerfile` files should be used, as shown at session below.

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

Finally, the image `josemottalopes/lirconsole` is pushed to the Dockerhub.

### Pulling and running at RPI

Below the docker command to load and run `lirconsole` at RPI. Please note that Lirc should be properly installed and configured according [RPI Setup instructions](https://github.com/josemotta/IoT.Starter.Pi.Lirc/blob/master/RPI_Setup.md).

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


