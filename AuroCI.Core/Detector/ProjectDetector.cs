using System.IO;
using System.Linq;
using AuroCI.Core.Models;

namespace AuroCI.Core.Detector;

public interface IProjectDetector
{
    ProjectConfig Detect(string path);
}

public class ProjectDetector : IProjectDetector
{
    public ProjectConfig Detect(string path)
    {
        var config = new ProjectConfig { ProjectPath = path };
        
        // Here it checks if in general this project exists on the device 
        if (!Directory.Exists(path))
        {
            return config;
        }
        
        // So here we will be looking for file with .csproj
        var csprojFile = Directory.GetFiles(path, "*.csproj").FirstOrDefault();
        
        // If it finds any file called like boom
        if (csprojFile.Any())
        {
            config.ProjectType = "C#";
        }

        return config;
    }
}