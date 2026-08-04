using AuroCI.Core.Models;

namespace AuroCI.Core.Detector;

public interface IProjectDetector
{
    ProjectConfig Detect(string path);
}

public class ProjectDetector : IProjectDetector
{
    private readonly (string Signature, string Type)[] _projectSignatures = 
    {
        ("<UseMaui>true</UseMaui>", "Maui"),
        ("Avalonia", "Avalonia"), 
        ("<UseWPF>true</UseWPF>", "WPF"),
        ("<UseWindowsForms>true</UseWindowsForms>", "WinForms"),
        ("Microsoft.AspNetCore.Components.WebAssembly", "BlazorWASM"),
        ("Microsoft.NET.Sdk.Web", "Web"),
        ("<OutputType>Exe</OutputType>", "Console"),
        ("<OutputType>WinExe</OutputType>", "Console"),
        ("Sdk=\"Microsoft.NET.Sdk.Worker\"", "Worker"),
        ("<OutputType>Library</OutputType>", "Library"),
    };

    public ProjectConfig Detect(string path)
    {
        var config = new ProjectConfig { ProjectPath = path, ProjectType = "Unknown" };
        
        config.ProjectName = Path.GetFileName(path);
        
        path = path.Trim().Replace("\\ ", " ");
        
        if (!Directory.Exists(path))
        {
            return config;
        }
        
        var csprojFile = Directory.GetFiles(path, "*.csproj").FirstOrDefault();
        
        if (csprojFile == null)
        {
            return config; 
        }
        
        try 
        {
            var fileContent = File.ReadAllText(csprojFile);
    
            foreach (var rule in _projectSignatures)
            {
                if (fileContent.Contains(rule.Signature))
                {
                    config.ProjectType = rule.Type;
                    return config;
                }
            }
        }
        catch (Exception)
        {
            config.ProjectType = "Unknown";
        }
        
        return config;
    }
}
