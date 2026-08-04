using AuroCI.Core.Templates;
using Spectre.Console;
using AuroCI.Core.Detector;
using static AuroCI.Core.Templates.MauiTemplate;

// Here it clears everything on the screen to make it look nice 
AnsiConsole.Clear();

// Logo of the CLI tool
AnsiConsole.Write(new FigletText("AuroCI").Centered().Color(Color.Green));

// Here some text to make it look fancy
AnsiConsole.MarkupLine("[bold white]Hello it's AuroCI[/] - your tool to automate CI/CD pipelines.");
AnsiConsole.MarkupLine("System status: [bold green]OK[/] - All systems are operational.\n");

string targetPath = string.Empty;

    while (true)
    {
        // Selection menu
         var selectionMode = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("How would you like to find the project?")
                .PageSize(10)
                .AddChoices(
                    "Enter the path manually",
                    "Detect project in current directory",
                    "Exit"));
    
        // Exit button
        if (selectionMode == "Exit")
        {
            AnsiConsole.Write(new Rule("[bold grey]See ya 😉[/]"));
            break; 
        }
    
        // Here we make logic if user choose 
        if (selectionMode == "Enter the path manually")
        {
            targetPath = AnsiConsole.Ask<string>("What is the project path?");
            if (targetPath == ".") targetPath = Directory.GetCurrentDirectory();
            
            // NEW SAFETY SYSTEM for manual path
            string fullPath = Path.GetFullPath(targetPath);
            if (!Directory.Exists(fullPath))
            {
                AnsiConsole.MarkupLine($"[bold red]Directory not found: {fullPath}[/]");
                AnsiConsole.MarkupLine($"[grey]Please touch any key to try again[/]");
                Console.ReadKey();
                continue; // Here it goes back, so user can try again
            }
            
            break;
        }
        else
        {
            var currentDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            bool goBackToMenu = false;
        
            // Cycle of the folders 
            while (true)
            {
                AnsiConsole.Clear();
            
                var directories = Directory.GetDirectories(currentDir) .Select(Path.GetFileName) .Where(name => !name.StartsWith(".")) .ToList();
            
                // Our navigation buttons
                directories.Insert(0, "✅ [green]Choose this directory[/]");
                directories.Insert(1, "⬅️ [yellow]Go back to menu[/]"); // Go back
            
                if (currentDir != "/") directories.Insert(2, "🔙 [yellow]Back (..)[/]");
            
                var selectedItem = AnsiConsole.Prompt(new SelectionPrompt<string>() 
                    .Title($"[cyan]Current directory:[/] {currentDir}\n[grey]Use arrows and Enter to select[/]") 
                    .PageSize(15) 
                    .AddChoices(directories));
            
                if (selectedItem == "✅ [green]Choose this directory[/]")
                {
                    targetPath = currentDir; 
                    break; 
                }
                else if (selectedItem == "⬅️ [yellow]Go back to menu[/]")
                {
                    goBackToMenu = true;
                    break; 
                }
                else if (selectedItem == "🔙 [yellow]Back (..)[/]")
                {
                    var parent = Directory.GetParent(currentDir);
                    if (parent != null) currentDir = parent.FullName; 
                }
                else
                {
                    currentDir = Path.Combine(currentDir, selectedItem); 
                }
                if(!goBackToMenu && !string.IsNullOrEmpty(targetPath)) break;
            }
            if(!goBackToMenu && !string.IsNullOrEmpty(targetPath)) break;
        }
        if (!string.IsNullOrEmpty(targetPath))
        {
            AnsiConsole.MarkupLine($"\n[bold green]Final path selected:[/] {targetPath}");
        }
    }
    if (string.IsNullOrWhiteSpace(targetPath))
    {
        return; 
    }
    var detector = new ProjectDetector();
    var config = detector.Detect(targetPath);
    AnsiConsole.MarkupLine($"Detected: {config.ProjectType}");
    var confirmed = AnsiConsole.Confirm("Do you want to generate CI/CD files?", false);
    if (!confirmed) return;

    // Here it choose which template was detected in project
    switch(config.ProjectType)
    {
        case "Maui":
            try
            {
                new MauiTemplate().Generate(config.ProjectName, targetPath);
                AnsiConsole.MarkupLine($"[green]Successfully generated CI/CD maui-ci.yml[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error generating MAUI template: {ex.Message}[/]");
            }
            break;
        
        case "Web":
            try
            {
                new WebTemplate().Generate(config.ProjectName, targetPath);
                AnsiConsole.MarkupLine($"[green]Successfully generated CI/CD web-ci.yml[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error generating Web template: {ex.Message}[/]");
            }
            break;
        
        case "Console":
            try
            {
                new ConsoleTemplate().Generate(config.ProjectName, targetPath);
                AnsiConsole.MarkupLine($"[green]Successfully generated CI/CD console-ci.yml[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error generating Console template: {ex.Message}[/]");
            }
            break;
        
        default:
            // If we can't detect the project type, we can ask the user to choose a template manually
            AnsiConsole.MarkupLine("[yellow]Be carefully: Project type not recognized. Please choose a template manually.[/]");
            var forceConfirm = AnsiConsole.Confirm("Do you want to generate a template manually?", false);
            
            if (!forceConfirm) return;
            AnsiConsole.MarkupLine($"[purple]Generating template manually.[/]");

            var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title($"[bold green]Choose project type:[/]")
                .PageSize(5)
                .HighlightStyle(new Style(foreground: Color.Cyan1))
                .AddChoices(new [] 
                    {
                        "🌐 ASP.NET Core Web",
                        "🖥️ .NET Console App",
                        "📱 .NET MAUI",
                        "❌ Exit"
                    }));
            try
            {
                switch (choice)
                {
                    case "🌐 ASP.NET Core Web":
                        new WebTemplate().Generate(config.ProjectName, targetPath);
                        AnsiConsole.MarkupLine($"[green]Successfully generated CI/CD web-ci.yml[/]");
                        break;
                    case "🖥️ .NET Console App":
                        new ConsoleTemplate().Generate(config.ProjectName, targetPath);
                        AnsiConsole.MarkupLine($"[green]Successfully generated CI/CD console-ci.yml[/]");
                        break;
                    case "📱 .NET MAUI":
                        new MauiTemplate().Generate(config.ProjectName, targetPath);
                        AnsiConsole.MarkupLine($"[green]Successfully generated CI/CD maui-ci.yml[/]");
                        break;
                    case "❌ Exit":
                        AnsiConsole.MarkupLine("[yellow]Exiting without generating any templates.[/]");
                        break;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error generating template: {ex.Message}[/]");
            }
            break;
    }
    AnsiConsole.MarkupLine("[bold red]WARNING!! Never trust CLI tools that automating CI/CD actions and check it yourself[/]");