using AuroCI.Core.Interfaces;

namespace AuroCI.Core.Templates;

public abstract class BaseTemplate : ITemplateGenerator
{
    public abstract string Name { get; } 

    protected abstract string GetYamlContent(string projectName);

    public void Generate(string projectName, string targetDirectory)
    {
        string githubWorkflowsDir = Path.Combine(targetDirectory, ".github", "workflows");
        
        if (!Directory.Exists(githubWorkflowsDir))
        {
            Directory.CreateDirectory(githubWorkflowsDir);
        }

        string yamlContent = GetYamlContent(projectName);
        
        string filePath = Path.Combine(githubWorkflowsDir, Name);
        File.WriteAllText(filePath, yamlContent);
    }
}