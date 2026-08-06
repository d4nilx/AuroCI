using Spectre.Console;

namespace AuroCI.Core.Helpers;

public static class DirectoryNavigator
{
    public static string? SelectDirectrory()
    {
        var curDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        while (true)
        {
            AnsiConsole.Clear();
            
            List<string> directories = new();

            try
            {
                directories = Directory.GetDirectories(curDirectory)
                    .Select(Path.GetFileName)
                    .Where(name => !string.IsNullOrEmpty(name) && !name.StartsWith(".")) // Захист від прихованих папок
                    .ToList();
            }
            catch (UnauthorizedAccessException)
            {
                AnsiConsole.MarkupLine("[red]No access to this directory.[/]");
                Console.ReadKey();
                
                var parentFallback = Directory.GetParent(curDirectory);
                if (parentFallback != null) curDirectory = parentFallback.FullName;
                continue;
            }
            
            directories.Insert(0, "✅ [green]Choose this directory[/]");
            directories.Insert(1, "⬅️ [yellow]Go back to menu[/]");
            
            if (curDirectory != "/") 
            {
                directories.Insert(2, "🔙 [yellow]Back (..)[/]");
            }

            var selectedItem = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[cyan]Current directory:[/] {curDirectory}\n[grey]Use arrows and Enter to select[/]")
                    .PageSize(15)
                    .AddChoices(directories));
            
            if (selectedItem == "✅ [green]Choose this directory[/]")
            {
                return curDirectory; 
            }
            
            if (selectedItem == "⬅️ [yellow]Go back to menu[/]")
            {
                return null; 
            }
            
            if (selectedItem == "🔙 [yellow]Back (..)[/]")
            {
                var parent = Directory.GetParent(curDirectory);
                if (parent != null) curDirectory = parent.FullName;
            }
            else
            {
                curDirectory = Path.Combine(curDirectory, selectedItem);
            }
        }
    }
}