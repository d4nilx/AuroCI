namespace AuroCI.Core.Templates;

public class PythonFastAPITemplate : BaseTemplate
{
    public override string Name => "python-fastapi-ci.yml";
    
    protected override string GetYamlContent(string projectName) => $@"name: {projectName} Python FastAPI

jobs:
  build:
    runs-on: ubuntu-latest  
    steps:
    - uses: actions/checkout@v4
    
    - uses: actions/setup-python@v5
      with:
        python-version: '3.12'
        
    - name: Install dependencies
      run: pip install -r requirements.txt
      
    - name: Run tests
      run: pytest
      
    - name: Check FastAPI app starts
      run: fastapi --version
";
}