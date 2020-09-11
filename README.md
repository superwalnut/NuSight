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

- Using `dotnet tool install -g NuSight` to install the console tool from nuget 

[![NuSight](./images/nuget.png)](https://www.nuget.org/packages/NuSight)

[NuSight](https://www.nuget.org/packages/NuSight)

```shell
$ dotnet tool install -g NuSight
```

---

### Usage

### nusight list

> a command can be used for your CI build to validate your nuget packages for the whole solution.

- --help or -h to list all parameters and descriptions

```shell
$ nusight list -h 
```

- --solution or -s to specify the selected solution path, if you haven't specified any solution path, it will take current directory as selected path

```shell
$ nusight list -s=<YOUR_PATH> 
```

- --inconsistency or -i to triage inconsistent nuget package versions installed in the selected solution

```shell
$ nusight list -s=<YOUR_PATH> -i
```

- --outdated or -o to triage outdated nuget packages installed in the selected solution

```shell
$ nusight list -s=<YOUR_PATH> -o
```

- --prereleased or -p to triage if any pre-released packages are installed in the selected solution

```shell
$ nusight list -s=<YOUR_PATH> -p
```

- --unpublished or -u to triage if any unpublished packages are installed in the selected solution.

```shell
$ nusight list -s=<YOUR_PATH> -u
```


If the triage finds any above errors, it will return with an error code, and your pipeline will fail. Otherwise return with a 0 code and pipeline will succeed.


### nusight update

> a command can be used for bulk updating packages in multiple projects under a solution folder.

- --help or -h to list all parameters and descriptions

```shell
$ nusight update -h 
```

- --solution or -s to specify the selected solution path, if you haven't specified any solution path, it will take current directory as selected path

```shell
$ nusight update -s=<YOUR_PATH> 
```

- --display or -d allows you only print 'dotnet update' commands on the screen, instead of executing it. If you don't specify this, update commands will be run and update packages in the selected solution.

```shell
$ nusight update -s=<YOUR_PATH> -d
```

### nusight remove

> a command can be used for bulk removing packages in multiple projects under a solution folder. 

- --help or -h to list all parameters and descriptions

```shell
$ nusight remove -h 
```

- --solution or -s to specify the selected solution path, if you haven't specified any solution path, it will take current directory as selected path

```shell
$ nusight remove -s=<YOUR_PATH> 
```

- --package or -p to specify the package name to be removed, if you want to select multiple packages, use comma separated values

```shell
$ nusight remove -s=<YOUR_PATH> -p=Serilog
```

- --display or -d allows you only print 'dotnet remove' commands on the screen, instead of executing it. If you don't specify this, remove commands will be run and remove packages in the selected solution.

```shell
$ nusight remove -s=<YOUR_PATH> -p=Serilog -d
```

### nusight clone

> a command can be used for cloning all nuget packages from a source solution/project to a target project, this is handy if you have a template project and its nuget packages will always be used for any future projects, you can always clone your them to the new projects.

- --help or -h to list all parameters and descriptions

```shell
$ nusight clone -h 
```

- --source or -s to specify the source solution path

```shell
$ nusight clone -s=<YOUR_PATH> 
```

- --target or -t to specify the target project path, it must be a csproj

```shell
$ nusight clone -s=<YOUR_PATH> -t=<TARGET_PATH>/<PROJECT>.csproj
```

- --display or -d allows you only print 'dotnet install' commands on the screen, instead of executing it. If you don't specify this, install commands will be run and install packages in the target project.

```shell
$ nusight clone -s=<YOUR_PATH> -t=<TARGET_PATH>/<PROJECT>.csproj -d
```

- --latest or -l to specify if you want to copy nuget packages from source solution with latest version.

```shell
$ nusight clone -s=<YOUR_PATH> -t=<TARGET_PATH>/<PROJECT>.csproj -l
```

### nusight export

> a command can be used for exporting nuget package reference as json file.

- --help or -h to list all parameters and descriptions

```shell
$ nusight export -h 
```

- --source or -s to specify the source solution path

```shell
$ nusight export -s=<YOUR_PATH> 
```

- --file or -f to specify the exported file path and name

```shell
$ nusight export -s=<YOUR_PATH> -f=<FILE_PATH>/<FILE_NAME>.json
```

### nusight import

> a command can be used for importing nuget packages from an exported json file.

- --help or -h to list all parameters and descriptions

```shell
$ nusight import -h 
```

- --solution or -s to specify the selected solution path to be imported to.

```shell
$ nusight import -s=<YOUR_PATH> 
```

- --file or -f to specify the exported file path and name

```shell
$ nusight import -s=<YOUR_PATH> -f=<FILE_PATH>/<FILE_NAME>.json
```

- --display or -d allows you only print 'dotnet install' commands on the screen, instead of executing it. If you don't specify this, install commands will be run and install packages in the target project.

```shell
$ nusight import -s=<YOUR_PATH> -f=<FILE_PATH>/<FILE_NAME>.json -d
```


### To use it with github actions

```shell
    - name: Nuget triage
      working-directory: ./MyProject
      run: | 
        dotnet tool install -g NuSight
        nusight list -o -p -u -i
```

### To use it with gitlab CI

```shell
build:
  stage: 'build'
  script: 
    - dotnet restore
    - dotnet build    
    - dotnet tool install -g NuSight
    - nusight list -o -p -u -i
  when: always  

```

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

TODO

