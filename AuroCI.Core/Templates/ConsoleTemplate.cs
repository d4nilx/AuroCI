using System.IO;
using AuroCI.Core.Interfaces;

namespace AuroCI.Core.Templates;

public class ConsoleTemplate : ITemplateGenerator
{
    public string Name { get; } = ".NET Console App CI/CD Template";

    public void Generate(string projectName, string targetDirectory)
    {
        var yaml = $@"name: {projectName} Console CI

on:
  push:
    branches: [ ""main"" ]
  pull_request:
    branches: [ ""main"" ]

jobs:
  build:
    strategy:
      matrix:
        # To turn off on of the OS simply delete it.
        os: [ubuntu-latest, windows-latest, macos-latest]
        
    runs-on: ${{{{ matrix.os }}}}
    
    steps:
    - name: Checkout Code
      uses: actions/checkout@v4
      
    - name: Setup .NET SDK
      uses: actions/setup-dotnet@v4
      with:
        # NOTE!! Since the project was made in .NET version 10, we need to use the latest version of .NET SDK, if u use the other just simply delete it and paste your version.
        dotnet-version: '10.0.x'
        
    - name: Restore dependencies
      run: dotnet restore
      
    - name: Build App
      run: dotnet build --no-restore -c Release
      
    - name: Run Tests
      run: dotnet test --no-build --verbosity normal";

        var workflowsDir = Path.Combine(targetDirectory, ".github", "workflows");
        Directory.CreateDirectory(workflowsDir);

        File.WriteAllText(Path.Combine(workflowsDir, "console-ci.yml"), yaml);
    }
}