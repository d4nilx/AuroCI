namespace AuroCI.Core.Models;

public class ProjectConfig
{
    public string ProjectName { get; set; } =  string.Empty;
    public string TargetDirectory { get; set; } = string.Empty;
    
    // In the future there will be programing language, framework, and other options to choose from. But right now it's this minimum. 
}