namespace AuroCI.Core.Templates.DotNet;

public class ClassLibraryTemplate : BaseTemplate
{
    public override string Name => "classlib-ci.yml";

    protected override string GetYamlContent(string projectName)
    {
        return $@"name: {projectName} Class Library CI

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
    - name: Checkout code
      uses: actions/checkout@v4

    - name: Setup .NET SDK
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '10.0.x'

    - name: Restore dependencies
      run: dotnet restore
      
    - name: Build Library
      run: dotnet build --no-restore -c Release

    - name: Run Tests
      run: dotnet test --no-build --verbosity normal

    - name: Pack NuGet Package
      if: matrix.os == 'ubuntu-latest'
      run: dotnet pack --no-build -c Release -o ./artifacts

    - name: Upload NuGet Artifact
      if: matrix.os == 'ubuntu-latest'
      uses: actions/upload-artifact@v4
      with:
        name: {{projectName}}-nuget-package
        path: ./artifacts";
    }
}