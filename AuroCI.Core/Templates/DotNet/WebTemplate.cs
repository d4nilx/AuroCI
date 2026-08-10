namespace AuroCI.Core.Templates.DotNet;

public class WebTemplate : BaseTemplate
{
    public override string Name => "web-ci.yml";

    protected override string GetYamlContent(string projectName)
    {
        return $@"name: {projectName} Web CI/CD

on: 
  push:
    branches: [""main""]
  pull_request:
    branches: [""main""]

jobs: 
  build:
    strategy: 
      fail-fast: false
      matrix: 
        # NOTE!! Here you can use all OS or choose one you targeting
        os: [ubuntu-latest, windows-latest, macos-latest]

    runs-on: ${{{{ matrix.os }}}}
    
    steps: 
    - name: Checkout code
      uses: actions/checkout@v4

    - name: Setup .NET SDK 
      uses: actions/setup-dotnet@v4
      with:
        # NOTE! Here u can change this version for your or latest available 
        dotnet-version: '10.0.x'
    
    - name: Restore dependencies
      run: dotnet restore

    - name: Build Web App 
      run: dotnet build --configuration Release --no-restore

    - name: Run Tests
      run: dotnet test --no-build --verbosity normal

    - name: Publish Web App
      run: dotnet publish -c Release -o ./publish

    - name: Upload Artifact
      uses: actions/upload-artifact@v4
      with: 
        # Unique name for the artifact
        name: web-app-artifact-${{{{ matrix.os }}}}
        path: ./publish/";
    }
}