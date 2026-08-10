namespace AuroCI.Core.Templates;

public abstract class PythonLibraryTemplate : BaseTemplate
{
    public override string Name => "python-library-ci.yml";
    
    protected override string GetYamlContent(string projectName) => $@"
name: {projectName} Python Library CI
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
    - uses: actions/checkout@v4
    
    - uses: actions/setup-python@v5
      with:
        python-version: '3.12'
        
    - name: Install dependencies
      run: pip install -r requirements.txt
      
    - name: Run tests
      run: pytest
";
}