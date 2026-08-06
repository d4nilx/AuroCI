using AuroCI.Core.Interfaces;

namespace AuroCI.Core.Templates;

public class DockerTemplate : ITemplateGenerator
{
    public string Name => "Dockerfile";
    
    private readonly string _projectType;
    private readonly List<string> _skipDirs = new() { "bin", "obj", "node_modules", ".git" };

    public DockerTemplate(string projectType)
    {
        _projectType = projectType;
    }

    public void Generate(string projectName, string targetDirectory)
    {
        var baseImage = _projectType switch
        {
            "Web" or "BlazorWASM" => "mcr.microsoft.com/dotnet/aspnet:10.0",
            "Maui" or "WPF" or "WinForms" or "Avalonia" => null,
            _ => "mcr.microsoft.com/dotnet/runtime:10.0"
        };

        if (baseImage == null)
            throw new InvalidOperationException($"Docker is not supported for {_projectType} projects.");

        var csprojFiles = FindCsprojFiles(targetDirectory).ToList();
        
        if (!csprojFiles.Any())
            throw new InvalidOperationException("No .csproj files found.");

        string projFile = Path.GetFileName(csprojFiles.First());
        string dllName = $"{Path.GetFileNameWithoutExtension(csprojFiles.First())}.dll";

        var expose = _projectType is "Web" or "BlazorWASM" ? "EXPOSE 8080" : "";

        string dockerfileContent = $@"FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore ""{projFile}""
RUN dotnet publish ""{projFile}"" -c Release -o /app/publish /p:UseAppHost=false

FROM {baseImage} AS final
WORKDIR /app
{expose}
COPY --from=build /app/publish .
ENTRYPOINT [""dotnet"", ""{dllName}""]";

        File.WriteAllText(Path.Combine(targetDirectory, "Dockerfile"), dockerfileContent);
    }

    private IEnumerable<string> FindCsprojFiles(string rootPath)
    {
        foreach (var file in Directory.GetFiles(rootPath, "*.csproj"))
            yield return file;

        foreach (var dir in Directory.GetDirectories(rootPath)
                     .Where(d => !_skipDirs.Contains(Path.GetFileName(d)) 
                                 && !Path.GetFileName(d).StartsWith(".")))
        {
            foreach (var file in FindCsprojFiles(dir))
                yield return file;
        }
    }
}