using Spectre.Console;
using AuroCI.Core.Templates;

namespace AuroCI.Core.Helpers; 

public static class DockerHelper
{
    public static void TryGenerateDockerfile(string projectName, string targetPath, string projectType)
    {
        try
        {
            new DockerTemplate(projectType).Generate(projectName, targetPath);
            AnsiConsole.MarkupLine("[green]Successfully generated Dockerfile[/]");
            AnsiConsole.MarkupLine("[red]WARNING!! Please review the generated Dockerfile before using it in production. There might be mistakes![/]");
            AnsiConsole.MarkupLine("[bold red]WARNING!! Never trust CLI tools that automate CI/CD actions and check it yourself[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error generating Dockerfile: {ex.Message}[/]");
        }
    }
}