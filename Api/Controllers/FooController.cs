using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NuSight.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using NuSight.Services.Interfaces;
using System.Threading.Tasks;
using NuSight.Models.Nuget;

namespace NuSight.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FooController : ControllerBase
    {        
        private readonly ILogger _logger;
        private readonly INugetService _nugetService;
        private readonly IProjectService _projectService;

        public FooController(ILogger logger, INugetService nugetService, IProjectService projectService)
        {
            _logger = logger;
            _nugetService = nugetService;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var package = await _nugetService.SearchByPackageName("nunit");
            //var behind = package.BehindCount("4.3.0");
            //var version = package.Versions.FirstOrDefault(x => x.SemanticVersion == "4.3.0");
            //var catalog = await _nugetService.GetNugetVersionSummary(version.Id);
            return Ok(package);
        }

        [HttpGet("discover")]
        public async Task<IActionResult> GetProjects()
        {
            var path = "/Users/kevinwang/netcore/dotnet-core-api-template/DotnetCoreApiDemo";
            var info = await _projectService.GetAllProjectFilesAsync(path);
            return Ok(info);
        }
    }
}
