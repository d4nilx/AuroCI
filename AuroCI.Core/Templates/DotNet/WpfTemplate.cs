namespace AuroCI.Core.Templates.DotNet;

public class WpfTemplate : BaseTemplate
{
  public override string Name => "wpf-ci.yml";

  protected override string GetYamlContent(string projectName)
  {
    return $@"name: {projectName} Wpf CI
on:
  push:
    branches: [ ""main"" ]
  pull_request:
    branches: [ ""main"" ]

jobs:
    build: 
        runs-on: windows-latest
        steps:
        - name: Checkout Code
          uses: actions/checkout@v4
          
        - name: Setup .NET SDK
          uses: actions/setup-dotnet@v4
          with:
            dotnet-version: '10.0.x'
            
        - name: Restore dependencies
          run: dotnet restore
          
        - name: Build App
          run: dotnet build --no-restore -c Release
          
        - name: Run Tests
          run: dotnet test --no-build --verbosity normal

        - name: Publish App
          run: dotnet publish -c Release -o ./publish
          
        - name: Upload Artifact
          uses: actions/upload-artifact@v4
          with:
            name: {{projectName}}-windows-app
            path: ./publish/";
  }
}