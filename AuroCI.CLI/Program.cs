using Spectre.Console;

// Here it clears the screen
AnsiConsole.Clear();

// Here it makes a big ASCII-logo
AnsiConsole.Write(new FigletText("AuroCI")
    .LeftJustified()
    .Color(Color.SpringGreen3));

// Make some text info about system success and status
AnsiConsole.Markup("[bold white]Hello it's AuroCI[/] - your tool to automate CI/CD pipelines and manage your projects with ease.");
AnsiConsole.Markup("System status: [bold green]OK[/] - All systems are operational.\n");

// Making new rule to exit 
var rule = new Rule("[bold grey]press any key to exit[/]");
rule.Justification = Justify.Left;
AnsiConsole.Write(rule);

Console.ReadKey();