using AuroCI.Core.Interfaces;
using System.IO;
using System;

namespace AuroCI.Core.Templates;

public class BasicTemplateGenerator : ITemplateGenerator
{
    public string Name => "Basic Test Generator";

    public void Generate(string projectName, string targetPath)
    {
        // Here it creates the full path 
        string fullProjectPath = Path.GetFullPath(Path.Combine(targetPath, projectName));
        
        // Here it makes directory
        Directory.CreateDirectory(fullProjectPath);
        
        // Here it is just a log to see that Core logic is working
        Console.WriteLine($"[Core log] ==> Directory created: {fullProjectPath}");
        Console.WriteLine($"[Project name] ==> Generate CI/CD files...");
    }
}