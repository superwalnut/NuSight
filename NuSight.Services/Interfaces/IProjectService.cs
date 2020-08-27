using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuSight.Models.Models;

namespace NuSight.Services.Interfaces
{
    public interface IProjectService
    {
        Task<List<PackageReference>> GetAllProjectFilesAsync(string path);
    }
}
