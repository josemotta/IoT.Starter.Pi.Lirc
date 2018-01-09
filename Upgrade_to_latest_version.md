# IoT.Home.Pi

#### Home Intelligence with Raspberry Pi

### Upgrading images to latest version 

Suppose that a new software should be pulled from the cloud and installed on your `Thing` device. For safety, consider that RPI just rebooted and is at a stable and known state. There are no containers running. We just pushed to the docker registry a new software version for home-ui, home-web and nginx-proxy.

As shown below, some images are already installed at RPI:

	root@lumi:~# docker images
	REPOSITORY                   TAG                 IMAGE ID            CREATED             SIZE
	josemottalopes/home-ui       latest              589665e965c4        2 weeks ago         233MB
	josemottalopes/nginx-proxy   latest              dfcc69831d22        2 weeks ago         87.9MB
	josemottalopes/home-web      latest              241bfb394cda        2 weeks ago         235MB

See below the upgrade commands to download and install the latest software version available at docker registry.

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

Now remove all containers, since they are related to the old version.

	root@lumi:~# docker rm $(docker ps -a -q)
	5df1f1f8d0e0
	398ba160a6bb
	d9e682be8359

Execute the images with latest version, creating new  containers.
	
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

But job is not finished yet, since there are dangling images that should be removed to free unused memory. Please see below there are images from  a couple weeks ago that were replaced in the upgrade process. 

	root@lumi:~# docker images -a
	REPOSITORY                   TAG                 IMAGE ID            CREATED             SIZE
	josemottalopes/nginx-proxy   latest              2d3d112a7057        3 hours ago         87.9MB
	josemottalopes/home-web      latest              2f0e5ae80303        3 hours ago         235MB
	josemottalopes/home-ui       latest              e232a3323f6a        3 hours ago         233MB
	josemottalopes/home-ui       <none>              589665e965c4        2 weeks ago         233MB
	josemottalopes/nginx-proxy   <none>              dfcc69831d22        2 weeks ago         87.9MB
	josemottalopes/home-web      <none>              241bfb394cda        2 weeks ago         235MB

It is necessary to select and remove these dangling images, as explained below in a couple steps. First command select only unused images, and it is used at second command to kill them.

	root@lumi:~# docker images -f dangling=true
	REPOSITORY                   TAG                 IMAGE ID            CREATED             SIZE
	josemottalopes/home-ui       <none>              589665e965c4        2 weeks ago         233MB
	josemottalopes/nginx-proxy   <none>              dfcc69831d22        2 weeks ago         87.9MB
	josemottalopes/home-web      <none>              241bfb394cda        2 weeks ago         235MB

	root@lumi:~# docker rmi $(docker images -f dangling=true -q)
	Untagged: josemottalopes/home-ui@sha256:67b10ee226a9ba8aa41abd9639cdddb34096b51475f3461ad7a2a487a8103d53
	Deleted: sha256:589665e965c468e3d673c85b8ff0bc92820aa8941aa328f83771a34160319fe2
	Deleted: sha256:aca51432841dbe12720af6abcd6d08c984cb60557f62975b034b4d705ce8ea81
	Untagged: josemottalopes/nginx-proxy@sha256:08ccedc3ada5104cf207265c96755dd97bede056a4b92ec73e08c2ac09b4bf41
	Deleted: sha256:dfcc69831d2226e0b95a9505ae8d2f51d7c553db6de78d1864731ca12979464a
	Deleted: sha256:8fe54c25f4c69c2cdbcf964ca18b4a6a69afab9e1b20bb08806866052bd32b75
	Deleted: sha256:09e525399f01f083db20cefff7dd9629da3499e605b31d3690eba538aa0985b1
	Untagged: josemottalopes/home-web@sha256:c2483c67935a7fa7f4a227788a9799056f016be456bb61f9619ad303f793dd12
	Deleted: sha256:241bfb394cda25dde265668ffa81244244548ad8589e30855f5d48a16721bbfc
	Deleted: sha256:0e292f1fe6549466dfa7029ea3f53f4bc7f709e686bf69ee433875778ddbbcc5

And only latest images are occupying memory space now, as shown below.

	root@lumi:~# docker images -a
	REPOSITORY                   TAG                 IMAGE ID            CREATED             SIZE
	josemottalopes/nginx-proxy   latest              2d3d112a7057        3 hours ago         87.9MB
	josemottalopes/home-web      latest              2f0e5ae80303        3 hours ago         235MB
	josemottalopes/home-ui       latest              e232a3323f6a        3 hours ago         233MB
