using AuroCI.Core.Templates;

public class RustCli : BaseTemplate
{
    public override string Name => "rust-cli.yml";

    protected override string GetYamlContent(string projectName) => $@"name: {projectName} Rust CLI CI
on:
  push:
    branches: [ """"main"""" ]
  pull_request:
    branches: [ """"main"""" ]

permissions:
  contents: read

env:
  CARGO_TERM_COLOR: always

concurrency:
  group: ${{{{ github.workflow }}}}-${{{{ github.ref }}}}
  cancel-in-progress: true

jobs:
  test:
    name: Test & Lint
    runs-on: ubuntu-latest
    timeout-minutes: 20
    steps:
      - uses: actions/checkout@v4
      
      - name: Install Rust toolchain
        uses: dtolnay/rust-toolchain@stable
        with:
          toolchain: stable
          components: clippy, rustfmt
          
      - name: Rust Cache
        uses: Swatinem/rust-cache@v2
        
      - name: Check formatting
        run: cargo fmt --all -- --check
        
      - name: Run Clippy lints
        run: cargo clippy --all-targets --all-features --locked -- -D warnings
        
      - name: Run Tests
        run: cargo test --all-features --locked

  build:
    name: Release Build Check
    runs-on: ubuntu-latest
    needs: test
    steps:
      - uses: actions/checkout@v4
      
      - name: Install Rust toolchain
        uses: dtolnay/rust-toolchain@stable
        with:
          toolchain: stable
          components: clippy, rustfmt
          
      - name: Rust Cache
        uses: Swatinem/rust-cache@v2
        
      - name: Build Release Binary
        run: cargo build --release --locked

  audit:
    name: Security Audit
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Cargo Audit
        uses: rustsec/audit-check@v2
        with:
          token: ${{{{ secrets.GITHUB_TOKEN }}}}
";
}