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
            var requirements = Directory.GetFiles(path, "requirements.txt").FirstOrDefault();
            var pyprojFile = Directory.GetFiles(path, "pyproject.toml").FirstOrDefault();

            if (requirements != null || pyprojFile != null)
            {
                config.ProjectType = DetectPythonType(requirements ?? pyprojFile!);
                return config;
            }
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
    
    private string DetectPythonType(string filePath)
    {
        var content = File.ReadAllText(filePath).ToLower();
        
        if (content.Contains("flask")) return "PythonFlask";
        if (content.Contains("django")) return "PythonDjango";
        if (content.Contains("fastapi")) return "PythonFastApi";
        if (content.Contains("pandas") || content.Contains("numpy") || content.Contains("jupyter")) return "PythonDataScience";
        
        return "PythonScript";
    }
}
