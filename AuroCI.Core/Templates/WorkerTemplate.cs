
namespace AuroCI.Core.Templates;

public class WorkerTemplate
{
    public void Generate(string projectName, string targetDirectory)
    {
        var yaml = $@"
name: {projectName} Worker CI
on:
  push:
    branches: [ ""main"" ]
  pull_request:
    branches: [ ""main"" ]

jobs:
  build: 
    runs-on: ubuntu-latest
    steps:
    - name: Checkout Code
      uses: actions/checkout@v4
      
    - name: Setup .NET SDK
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '10.0.x'
        
    - name: Restore dependencies
      run: dotnet restore
      
    - name: Build Worker
      run: dotnet build --no-restore -c Release
      
    - name: Run Tests
      run: dotnet test --no-build --verbosity normal
";
        
        var workflowsDir = Path.Combine(targetDirectory, ".github", "workflows");
        Directory.CreateDirectory(workflowsDir);
        
        File.WriteAllText(Path.Combine(workflowsDir, "worker-ci.yml"), yaml);
    }
}