# IoT.Home.Pi


Home Intelligence with Raspberry Pi

Considering some images are already installed at RPI:

	root@lumi:~# docker images
	REPOSITORY                   TAG                 IMAGE ID            CREATED             SIZE
	josemottalopes/home-ui       latest              589665e965c4        2 weeks ago         233MB
	josemottalopes/nginx-proxy   latest              dfcc69831d22        2 weeks ago         87.9MB
	josemottalopes/home-web      latest              241bfb394cda        2 weeks ago         235MB

When a new version is available for the device,  the new `Thing` can be pulled from the cloud. See below an upgrade based on the latest version available at docker registry.

	root@lumi:~# docker pull josemottalopes/nginx-proxy
	Using default tag: latest
	latest: Pulling from josemottalopes/nginx-proxy
	cd8b673adb84: Already exists
	1e2b2afc1dc6: Already exists
	8877327663c1: Already exists
	30769b69127d: Already exists
	566ebde5ccae: Already exists
	41a021a94b20: Pull complete
	865d577a6364: Pull complete
	Digest: sha256:dc086ce9b61af06f93c4827d229f3a5d086aedcef076b769612489dac402b9df
	Status: Downloaded newer image for josemottalopes/nginx-proxy:latest
	
	root@lumi:~# docker pull josemottalopes/home-ui
	Using default tag: latest
	latest: Pulling from josemottalopes/home-ui
	0d9fbbfaa2cd: Already exists
	b015fdc7d33a: Already exists
	60aaa226f085: Already exists
	01963091a185: Already exists
	09d3726a2ea3: Already exists
	b948813a76bf: Pull complete
	Digest: sha256:89cc4656e7d9697721b451d69fc4adfe229376d99aedd52e72bf852d7a40fc66
	Status: Downloaded newer image for josemottalopes/home-ui:latest

	root@lumi:~# docker pull josemottalopes/home-web
	Using default tag: latest
	latest: Pulling from josemottalopes/home-web
	0d9fbbfaa2cd: Already exists
	b015fdc7d33a: Already exists
	60aaa226f085: Already exists
	01963091a185: Already exists
	e487938a6461: Already exists
	19c6008d5375: Pull complete
	Digest: sha256:18c0b8ad3a97f619562b9b6815af47f2acf72f90ba15731eb995d7fbcd55669d
	Status: Downloaded newer image for josemottalopes/home-web:latest

Remove all images.

	root@lumi:~# docker rm $(docker ps -a -q)
	5df1f1f8d0e0
	398ba160a6bb
	d9e682be8359

Execute images.
	
	alias yhomeui='docker run --privileged -p 80:80 -d josemottalopes/home-ui:latest'
	alias yhomeweb='docker run --privileged -p 5010:5010 -d josemottalopes/home-web:latest'
	alias yproxy='docker run --privileged -p 443:443 -d josemottalopes/nginx-proxy:latest'
	root@lumi:~# yproxy
	0680e32f0ae6895fd8c0a3f117d752539c2e8c350a398168853efa67464e111c
	root@lumi:~# yhomeweb
	fa14eccb842b1c2b99c30c24c1277c0bd11c9db13e91fa2638d948bf4981d414
	root@lumi:~# yhomeui
	e8944ab410da9a3cb3c76be105d4ec4f4804872fbbbe49555205e319c974af5d

The running containers are shown below.

	root@lumi:~# docker ps -a
	CONTAINER ID        IMAGE                               COMMAND                  CREATED             STATUS              PORTS                            NAMES
	e8944ab410da        josemottalopes/home-ui:latest       "dotnet Home.UI.dll"     25 seconds ago      Up 17 seconds       0.0.0.0:80->80/tcp               pensive_beaver
	fa14eccb842b        josemottalopes/home-web:latest      "dotnet IO.Swagger..."   33 seconds ago      Up 30 seconds       80/tcp, 0.0.0.0:5010->5010/tcp   reverent_neumann
	0680e32f0ae6        josemottalopes/nginx-proxy:latest   "nginx -g 'daemon ..."   47 seconds ago      Up 44 seconds       80/tcp, 0.0.0.0:443->443/tcp     vigilant_neumann
	root@lumi:~#
	
