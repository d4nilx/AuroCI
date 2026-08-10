using AuroCI.Core.Templates.DotNet;

namespace AuroCI.Core.Templates.Python;

public class PythonDataScienceTemplate : BaseTemplate
{
  public override string Name => "python-datascience-ci.yml";

  protected override string GetYamlContent(string projectName) => $@"
name: {projectName} Python Data Science CI

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

    - name: Set up Python
      uses: actions/setup-python@v5
      with:
        python-version: '3.12'

    - name: Install dependencies
      run: pip install -r requirements.txt

    - name: Run tests
      run: pytest

    # It's not full YAML for Python Data Science because Data Science projects can vary widely in structure and requirements. You may need to customize this template further based on your specific project needs.
";
}