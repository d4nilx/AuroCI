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
        if (csprojFile != null)
        {
            config.ProjectType = "C#";
        }
        
        // Here it takes first file available 
        var csprojPath = csprojFile;
        
        //Here make it read full file which it finds 
        var fileContent = File.ReadAllText(csprojPath.ToString());
        
        // Here it detects the MAUI project
        if (fileContent.Contains("<UseMaui>true</UseMaui>")) config.ProjectType = "Maui";
        
        // Here it detects the web project 
        if (fileContent.Contains("Microsoft.NET.Sdk.Web")) config.ProjectType = "Web";
        
        // Here it detects console projects 
        if (fileContent.Contains("Microsoft.NET.Sdk")) config.ProjectType = "Console";
        
        // In case it's not any of the above it will be unknown
        else config.ProjectType = "Unknown";
        
        return config;
    }
}