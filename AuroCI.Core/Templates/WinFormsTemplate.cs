using System.IO;
using AuroCI.Core.Interfaces;

namespace AuroCI.Core.Templates;

public class WinFormsTemplate : BaseTemplate
{
  public override string Name => "winforms-ci.yml";

  protected override string GetYamlContent(string projectName)
    {
        return $@"name: {projectName} WinForms CI
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
          run: dotnet test --no-build --verbosity normal";
    }
}