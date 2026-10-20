using AuroCI.Core.Templates;

namespace AuroCI.Core.Templates.Rust;

public class RustLibrary : BaseTemplate
{
    public override string Name => "rust-library.yml";

    protected override string GetYamlContent(string projectName) => $@"name: {{projectName}} Rust Library CI
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
  group: ${{{{{{{{ github.workflow }}}}}}}}-${{{{{{{{ github.ref }}}}}}}}
  cancel-in-progress: true

jobs:
  test:
    runs-on: ubuntu-latest
    timeout-minutes: 20
    strategy:
      matrix:
        toolchain: [ stable, beta ]
    steps:
      - uses: actions/checkout@v4
      - uses: dtolnay/rust-toolchain@master
        with:
          toolchain: ${{{{{{{{ matrix.toolchain }}}}}}}}
          components: clippy, rustfmt
      - uses: Swatinem/rust-cache@v2
      - run: cargo fmt -- --check
      - run: cargo clippy --all-features --locked -- -D warnings
      - run: cargo test --all-features --locked
      - run: cargo doc --no-deps --locked
        env:
          RUSTDOCFLAGS: -D warnings

  audit:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: rustsec/audit-check@v2
        with:
          token: ${{{{{{{{ secrets.GITHUB_TOKEN }}}}}}}}
";
}