
namespace AuroCI.Core.Templates.DotNet;

public class BlazorTemplate : BaseTemplate
{
  public override string Name => "blazor-ci.yml";

  protected override string GetYamlContent(string projectName)
  {
    return $@"
name: {projectName} Blazor CI
on:
  push:
    branches: [ ""main"" ]
  pull_request:
    branches: [ ""main"" ]

jobs:
  build: 
    strategy: 
      fail-fast: false
      matrix:
        os: [ubuntu-latest, windows-latest, macos-latest]
    runs-on: ${{{{ matrix.os }}}}
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