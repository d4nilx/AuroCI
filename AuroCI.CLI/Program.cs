using AuroCI.Core.Templates;
using Spectre.Console;
using AuroCI.Core.Detector;

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
            // Calling Maui
            break;
        case "Web":
            // Calling Web
            break;
        case "Console":
            // Calling Console
            break;
        default:
            // unknown type
            break;
    }