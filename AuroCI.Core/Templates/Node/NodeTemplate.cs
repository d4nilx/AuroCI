namespace AuroCI.Core.Templates.Node;

public enum NodeProjectType
{
    General,
    Next,
    Angular,
    Vue,
    Nest
}

public class NodeTemplate : BaseTemplate
{
    private readonly NodeProjectType _type;

    public NodeTemplate(NodeProjectType type = NodeProjectType.General)
    {
        _type = type;
    }

    public override string Name => _type == NodeProjectType.General 
        ? "node-cicd.yml" 
        : $"{_type.ToString().ToLower()}-cicd.yml";

    protected override string GetYamlContent(string projectName)
    {
        return $"""
                name: {projectName} Node.js CI/CD

                on:
                  push:
                    branches: [ "main" ]
                  pull_request:
                    branches: [ "main" ]

                jobs:
                  build:
                    runs-on: ubuntu-latest

                    steps:
                    - uses: actions/checkout@v4

                    - name: Set up Node.js
                      uses: actions/setup-node@v4
                      with:
                        node-version: '20'
                        cache: 'npm'

                    - name: Install dependencies
                      run: npm ci

                    - name: Run tests
                    run: npm run test --if-present
                
                    - name: Build for production
                    run: npm run build --if-present
                      
                    - name: Upload Artifact
                      uses: actions/upload-artifact@v4
                      with:
                        name: {projectName}-build
                        path: ./dist
                """;
    }
}