namespace AuroCI.Core.Models;

public class ProjectConfig
{
    public string ProjectPath { get; set; } =  string.Empty;
    public string ProjectType { get; set; } = "Unknown";
    
    public string ProjectName { get; set; } = string.Empty;
    public bool IsDetected => ProjectType != "Unknown";
}
