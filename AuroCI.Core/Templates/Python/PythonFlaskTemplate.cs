using AuroCI.Core.Templates.DotNet;

namespace AuroCI.Core.Templates.Python;

public class PythonFlaskTemplate : BaseTemplate
{
    public override string Name => "python-flask-ci.yml";
    
    protected override string GetYamlContent(string projectName) => $@"name: {projectName} Python Flask

on:
  push:
    branches: [ ""main"" ]
  pull_request:
    branches: [ ""main"" ]

jobs:
  build:
    runs-on: ubuntu-latest  
    steps:
    - uses: actions/checkout@v4
    
    - uses: actions/setup-python@v5
      with:
        python-version: '3.12'
        cache: 'pip'
        
    - name: Install dependencies
      run: pip install -r requirements.txt
      
    - name: Run tests
      run: pytest
      
    - name: Check Flask app starts
      run: flask --version
";
}