# Lirc-Console

#### IoT.Starter.Pi.Thing powered by Linux Infrared Remote Control

#### lirc-compose.yml  

dockerfile: Lirc/lirc.Dockerfile

image: josemottalopes/lirconsole

**Links:**  
https://hub.docker.com/r/josemottalopes/lirconsole/
  

#### x64: Build 

	cd lirc-console\Console
	docker-compose -f lirc-compose.yml build   
	docker push josemottalopes/lirconsole:latest  

#### arm: RaspberryPi 

	docker run -it --name home-lirc --privileged -v /var/run/lirc:/var/run/lirc josemottalopes/lirconsole:latest 



