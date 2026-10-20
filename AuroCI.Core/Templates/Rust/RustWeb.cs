using AuroCI.Core.Templates;

namespace AuroCI.Core.Templates.Rust;

public class RustWeb : BaseTemplate
{
    public override string Name => "rust-web-cicd.yml";

    protected override string GetYamlContent(string projectName) => $@"
name: {projectName} Rust Web CI
on:
    push:  
        branches: [ """"main"""" ]
    pull_request:
        branches: [ """"main"""" ]

jobs: 
    build:
        runs-on: ubuntu-latest
        steps:
        - name: Checkout Code
          uses: actions/checkout@v4
    
        - name: Set up Rust
          uses: dtolnay/rust-toolchain@stable
          with:
            components: clippy, rustfmt

        - name: Restore/Cache dependencies 
          uses: Swatinem/rust-cache@v2

        - name: Check Formating
          run: cargo fmt -- --check

        - name: Run Linter 
          run: cargo clippy -- -D warnings

        - name: Build API
          run: cargo build --release

        - name: Run Tests
          run: cargo test

        - name: Upload Artifact 
          uses: actions/upload-artifact@v4
          with:
            name: {projectName}-linux-web
";
}
