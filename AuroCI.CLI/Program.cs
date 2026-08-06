using AuroCI.Core.Templates;
using Spectre.Console;
using AuroCI.Core.Detector;
using AuroCI.Core.Helpers; 
using AuroCI.Core.Models;
using AuroCI.Core.Interfaces;

var templates = new Dictionary<string, (ITemplateGenerator template, string fileName)>
{
    ["Maui"]         = (new MauiTemplate(),         "maui-ci.yml"),
    ["Web"]          = (new WebTemplate(),           "web-ci.yml"),
    ["Console"]      = (new ConsoleTemplate(),       "console-ci.yml"),
    ["Avalonia"]     = (new AvaloniaTemplate(),      "avalonia-ci.yml"),
    ["WPF"]          = (new WpfTemplate(),           "wpf-ci.yml"),
    ["WinForms"]     = (new WinFormsTemplate(),      "winforms-ci.yml"),
    ["BlazorWASM"]   = (new BlazorTemplate(),        "blazor-ci.yml"),
    ["ClassLibrary"] = (new ClassLibraryTemplate(),  "classlib-ci.yml"),
    ["Worker"]       = (new WorkerTemplate(),        "worker-ci.yml"),
};

var manualTemplates = new Dictionary<string, (ITemplateGenerator template, string fileName)>
{
    ["🌐 ASP.NET Core Web"]  = (new WebTemplate(), "web-ci.yml"),
    ["🖥️ .NET Console App"] = (new ConsoleTemplate(), "console-ci.yml"),
    ["📱 .NET MAUI"]        = (new MauiTemplate(), "maui-ci.yml"),
    ["🎨 Avalonia UI"]      = (new AvaloniaTemplate(), "avalonia-ci.yml"),
    ["🪟 WPF"]              = (new WpfTemplate(), "wpf-ci.yml"),
    ["🖼️ WinForms"]         = (new WinFormsTemplate(), "winforms-ci.yml"),
    ["⚛️ Blazor WASM"]       = (new BlazorTemplate(), "blazor-ci.yml"),
    ["📚  ClassLibrary"]    = (new ClassLibraryTemplate(), "classlib-ci.yml"),
    ["💻  Worker"]          = (new WorkerTemplate(), "worker-ci.yml")
};

// Main program cycle
while (true)
{
    AnsiConsole.Clear();
    AnsiConsole.Write(new FigletText("AuroCI").Centered().Color(Color.Green));
    AnsiConsole.MarkupLine("[bold white]Hello it's AuroCI[/] - your tool to automate CI/CD pipelines.");
    AnsiConsole.MarkupLine("System status: [bold green]OK[/] - All systems are operational.\n");

    string targetPath = string.Empty;
    ProjectDetector? detector;
    ProjectConfig? config;

    while (true)
    {
        var selectionMode = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("How would you like to find the project?")
                .PageSize(10)
                .AddChoices(
                    "Enter the path manually",
                    "Detect project in current directory",
                    "Create a Dockerfile",
                    "Exit"));

        if (selectionMode == "Exit")
        {
            AnsiConsole.Write(new Rule("[bold grey]See ya 😉[/]"));
            return;
        }

        if (selectionMode == "Enter the path manually")
        {
            targetPath = AnsiConsole.Ask<string>("What is the project path?");
            if (targetPath == ".") targetPath = Directory.GetCurrentDirectory();

            if (!Directory.Exists(Path.GetFullPath(targetPath)))
            {
                AnsiConsole.MarkupLine($"[bold red]Directory not found: {Path.GetFullPath(targetPath)}[/]");
                AnsiConsole.MarkupLine($"[grey]Please touch any key to try again[/]");
                Console.ReadKey();
                continue; 
            }
            break;
        }

        if (selectionMode == "Create a Dockerfile")
        {
            targetPath = AnsiConsole.Ask<string>("What is the project path for Dockerfile?");
            if (targetPath == ".") targetPath = Directory.GetCurrentDirectory();

            if (!Directory.Exists(Path.GetFullPath(targetPath)))
            {
                AnsiConsole.MarkupLine($"[bold red]Directory not found: {Path.GetFullPath(targetPath)}[/]");
                AnsiConsole.MarkupLine($"[grey]Please touch any key to try again[/]");
                Console.ReadKey();
                continue; 
            }

            detector = new ProjectDetector();
            config = detector.Detect(targetPath);
            AnsiConsole.MarkupLine($"Detected: {config.ProjectType}");

            DockerHelper.TryGenerateDockerfile(config.ProjectName, targetPath, config.ProjectType);

            var doAnotherDocker = AnsiConsole.Confirm("\nDo you want to process another project?", false);
            if (!doAnotherDocker)
            {
                AnsiConsole.Write(new Rule("[bold grey]See ya 😉[/]"));
                return; 
            }
            continue;
        }
        
        if (selectionMode == "Detect project in current directory")
        {
            var selectedPath = DirectoryNavigator.SelectDirectrory();
            if (selectedPath == null) continue; 
            
            targetPath = selectedPath;
            break; 
        }
    }

    if (string.IsNullOrWhiteSpace(targetPath)) return;

    AnsiConsole.MarkupLine($"\n[bold green]Final path selected:[/] {targetPath}");
    
    detector = new ProjectDetector();
    config = detector.Detect(targetPath);
    AnsiConsole.MarkupLine($"Detected: {config.ProjectType}");
    
    var confirmed = AnsiConsole.Confirm("Do you want to generate CI/CD files?", false);
    if (!confirmed) continue;

    if (templates.TryGetValue(config.ProjectType, out var entry))
    {
        try
        {
            entry.template.Generate(config.ProjectName, targetPath);
            AnsiConsole.MarkupLine($"[green]Successfully generated CI/CD {entry.fileName}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
    }
    else
    {
        AnsiConsole.MarkupLine("[yellow]Be carefully: Project type not recognized. Please choose a template manually.[/]");
        var forceConfirm = AnsiConsole.Confirm("Do you want to generate a template manually?", false);

        if (!forceConfirm)
        {
            var stay = AnsiConsole.Confirm("Do you want to stay in the menu to choose another project?", false);
            if (stay) continue;
            
            AnsiConsole.Write(new Rule("[bold grey]See ya 😉[/]"));
            return;
        }

        AnsiConsole.MarkupLine($"[purple]Generating template manually.[/]");

        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title($"[bold green]Choose project type:[/]")
            .PageSize(10)
            .HighlightStyle(new Style(foreground: Color.Cyan1))
            .AddChoices(manualTemplates.Keys.Append("❌ Exit"))); 
            
        if (choice == "❌ Exit")
        {
            AnsiConsole.MarkupLine("[yellow]Exiting without generating any templates.[/]");
        }
        else if (manualTemplates.TryGetValue(choice, out var selected))
        {
            try
            {
                selected.template.Generate(config.ProjectName, targetPath);
                AnsiConsole.MarkupLine($"[green]Successfully generated CI/CD {selected.fileName}[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error generating template: {ex.Message}[/]");
            }
        }
    }

    var generateDocker = AnsiConsole.Confirm("Would you also like to generate a Dockerfile?", false);
    if (generateDocker)
    {
        DockerHelper.TryGenerateDockerfile(config.ProjectName, targetPath, config.ProjectType);

        var doAnother = AnsiConsole.Confirm("\nDo you want to process another project?", false);
        if (!doAnother)
        {
            AnsiConsole.Write(new Rule("[bold grey]See ya 😉[/]"));
            break;
        }
    }
}