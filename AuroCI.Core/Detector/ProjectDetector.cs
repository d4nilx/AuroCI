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
        
        if (csprojFile == null)
        {
            config.ProjectType = "Unknown";
            return config;
        }
        
        // If it finds any file called like boom
        try 
        {
            var csprojPath = csprojFile;
            var fileContent = File.ReadAllText(csprojPath.ToString());
    
            if (fileContent.Contains("<UseMaui>true</UseMaui>")) config.ProjectType = "Maui";
            else if (fileContent.Contains("Microsoft.NET.Sdk.Web")) config.ProjectType = "Web";
            else if (fileContent.Contains("<OutputType>Exe</OutputType>") || fileContent.Contains("<OutputType>WinExe</OutputType>")) config.ProjectType = "Console";
            else config.ProjectType = "Unknown";
        }
        catch (Exception)
        {
            // If the file can't be read (permissions, etc.), fall back to Unknown
            config.ProjectType = "Unknown";
        }
        
        return config;
    }
}