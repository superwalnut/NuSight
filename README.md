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

---

### Usage



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

