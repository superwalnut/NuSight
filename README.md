# NuSight - a .net core tool to triage your solution nuget packages

> This is a .net tool that analyze your solution folder, discover all your project files and diagnose all the nuget packages if they require attention, such as any project contains outdated packages or inconsistent package versions, etc. And it can be used as validation process by your CI/CD pipelines.

**Pre-request**

- .Net Core 3.1

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Support](#support)
- [License](#license)

---

## Features

> NuSight is analyzing solution and discovering all project nuget packages.

- list command: 
    analyze nuget packages if contains:
    - outdated packages within the same solution
    - inconsistent package versions within the same solution
    - pre-released packages within the same solution
    - unpublished packages within the same solution

- export command:
    export all package references from the selected solution folder into a json file

- import command:
    import all pakcages and install them from selected json file.

- update command:
    update all outdated packages for selected solution.

- delete command:
    delete all targeted packages for selected solution and all its projects.

- clone command:
    copy all nuget packages references and install them to targeted project.

---

## Installation

- Using `dotnet tool install -g NuSight` to install the console tool from nuget [NuSight](https://www.nuget.org/packages/Superwalnut.RedisClusterTemplate)

```shell
$ dotnet tool install -g NuSight
```

You should see 'Redis .Net Core Template' in your template list by `dotnet new -l`

![Redis .net core Template Screenshot](images/dotnet-new-l.png)

- Using `dotnet new redis-dotnet-core -n <your-project-name>` to create a project with your own project name using this template

```shell
$ dotnet new redis-dotnet-core -n RedisDemo -o RedisDemo
```

This creates a project in folder `RedisDemo`

![Redis Demo](images/redis-demo.png)

---

### Usage

- Default - 1 master + 1 slave + 1 api

> Go to the <project-folder>/docker, you will see `docker-compose.yml` file, this is where you can run

```shell
$ docker-compose up --build
```

- Swarm Mode - 1 master + 3 slaves + 1 api

> Run `docker-compose` with compatibility mode, it will apply preset `deploy` to run docker swarm mode 

```json
    deploy:
      replicas: 3
```

```shell
$ docker-compose --compatibility up --build
```

- Advanced - n-Master + n-Slaves + api

> To run n number of masters and slaves, you will need to configure a couple of things in `docker-compose.yml`

Configure connection strings in api container, add n-number of master and n-number of slaves connection strings

```json
    - Redis__0=docker_redis-master_1:6379
    - Redis__1=docker_redis-replica_1:6379
    - Redis__2=docker_redis-replica_2:6379
    - Redis__3=docker_redis-replica_3:6379
```

Add or configure `deploy` section to the number you wanted

```json
    deploy:
    replicas: 3
```

---

## Documentation

- TODO

---

## Support

Reach out to me at one of the following places!

- [Follow me @ Github](https://github.com/superwalnut)

- [Twitter](https://twitter.com/superwalnuts)

- [![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Z8Z61I9HB)

---

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)

- **[MIT license](http://opensource.org/licenses/mit-license.php)**

-------

## Reference

- docker stop all

ocker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)

- run command
docker-compose up --detach --scale redis-master=1 --scale redis-replica=3

docker-compose --compatibility up --build -p redis

- pack the project template
dotnet pack

dotnet build -c Release

- install template

dotnet new -i <package>

- create project

dotnet new redis-dotnet-core -n MyProject --force

- stop docker 
docker ps -q | xargs -L1 docker stop

- start docker
open --background -a Docker


- generate dev-cert ssl
dotnet dev-certs https --clean

dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p password

dotnet dev-certs https --trust

- Console App

list:
-h --help: help information
-s --solution: specify solution folder & name
-u --update: print update command for out of dated packages
-i --install: print install command for all packages


-o --output: save project packages to a file (generate a list of dotnet add package...)


-d --diagnose: inconsistent versions packages




- Local Web App


- Online App - csproj analyze
To upload your csproj

- Integrated with github
Github action to check update and automatically update packages


