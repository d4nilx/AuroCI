<p align="center">
  <img src=".github/assets/header.svg" alt="AuroCI Logo">
</p>

> A smart, interactive CLI tool for automated CI/CD pipeline generation — because writing YAML by hand is so 2020.

[![NuGet Version](https://img.shields.io/nuget/v/AuroCI?style=flat-square&color=00F2FF&label=nuget)](https://www.nuget.org/packages/AuroCI)
[![NuGet Downloads](https://img.shields.io/nuget/dt/AuroCI?style=flat-square&color=32D74B&label=downloads)](https://www.nuget.org/packages/AuroCI)
[![Stars](https://img.shields.io/github/stars/d4nilx/AuroCI?style=flat-square&color=FFD60A)](https://github.com/d4nilx/AuroCI/stargazers)
[![Visitors](https://hits.sh/github.com/d4nilx/AuroCI.svg?style=flat-square&label=visitors&color=orange)](https://hits.sh/github.com/d4nilx/AuroCI/)
[![CI](https://img.shields.io/github/actions/workflow/status/d4nilx/AuroCI/console-ci.yml?style=flat-square&label=CI)](https://github.com/d4nilx/AuroCI/actions)
[![Platform](https://img.shields.io/badge/platform-macOS%20%7C%20Windows%20%7C%20Linux-32D74B?style=flat-square)](https://github.com/d4nilx/AuroCI)
[![.NET](https://img.shields.io/badge/.NET-10.0-00F2FF?style=flat-square)](https://dotnet.microsoft.com)

---

## 📌 About

**AuroCI** is a modern CLI tool that eliminates the boilerplate of setting up GitHub Actions. It automatically scans your project directory, detects the .NET project type, and generates a fully configured multi-platform CI/CD workflow in seconds.

No config files. No YAML knowledge required. Just run `aci` and you're done.

### 🌟 Features

- 🔍 **Smart Auto-Detection** — parses `.csproj` files to identify your framework automatically.
- 🛠 **Zero Configuration** — generates complete `.github/workflows/` YAML files instantly.
- 🌐 **Broad Framework Support** — covers 90%+ of the .NET ecosystem out of the box.
- 🍎 **Cross-Platform CI/CD** — automatically configures multi-OS matrix builds (Ubuntu, macOS, Windows).
- 🎨 **Beautiful Terminal UI** — interactive experience powered by Spectre.Console.
- 🛡️ **Defensive Design** — safe file operations, interactive confirmations, and robust error handling.
- 🔧 **Manual Fallback** — unknown project type? Choose a template manually from an interactive menu.

---

## 🚀 Quick Start

**AuroCI** is available as a .NET Global Tool on NuGet. Install it globally with a single command:

```bash
dotnet tool install -g AuroCI
```

Then run it from any .NET project directory:

```bash
aci
```

That's it. AuroCI will detect your project type and generate the CI/CD pipeline automatically.

---

## 📦 Supported Project Types

| Project Type | Detected By | CI Runners |
|---|---|---|
| ASP.NET Core Web | `Microsoft.NET.Sdk.Web` | Ubuntu, macOS, Windows |
| .NET Console App | `<OutputType>Exe</OutputType>` | Ubuntu, macOS, Windows |
| .NET MAUI | `<UseMaui>true</UseMaui>` | macOS (iOS), Windows (Android) |
| Avalonia UI | `Avalonia` reference | Ubuntu, macOS, Windows |
| WPF | `<UseWPF>true</UseWPF>` | Windows only |
| WinForms | `<UseWindowsForms>true</UseWindowsForms>` | Windows only |
| Blazor WASM | `WebAssembly` reference | Ubuntu, macOS, Windows |
| Class Library | SDK-style, no `OutputType` | Ubuntu, macOS, Windows |
| Worker Service | `Microsoft.NET.Sdk.Worker` | Ubuntu|
| Flask Web App | `flask` in requirements.txt | Ubuntu only |
| Django Web App | `django` in requirements.txt | Ubuntu only |
| FastAPI | `fastapi` in requirements.txt | Ubuntu only |
| Python Script/Library | `requirements.txt` or `pyproject.toml` | Ubuntu, macOS, Windows |
| Python Data Science | `pandas`/`numpy`/`jupyter` | Ubuntu only |
| Node.js / Express | `express` in package.json | Ubuntu only |
| NestJS | `@nestjs/core` in package.json | Ubuntu only |
| Next.js | `next` in package.json | Ubuntu, macOS, Windows |
| Angular | `@angular/core` in package.json | Ubuntu, macOS, Windows |
| Vue.js | `vue` in package.json | Ubuntu, macOS, Windows |
| Node.js Script | `package.json` | Ubuntu, macOS, Windows |

---

## 🏗️ Project Structure

```
AuroCI.sln
├── AuroCI.Core/                # Pure C# logic — detectors, models, generators
│   ├── Detector/
│   │   └── ProjectDetector.cs
│   ├── Interfaces/
│   │   └── ITemplateGenerator.cs
│   ├── Models/
│   │   └── ProjectConfig.cs
│   └── Templates/
│           ├──DotNet/
|           |    ├── AvaloniaTemplate.cs
│           |    ├── BlazorTemplate.cs
│           |    ├── ClassLibraryTemplate.cs
│           |    ├── ConsoleTemplate.cs
│           |    ├── MauiTemplate.cs
│           |    ├── WebTemplate.cs
│           |    ├── WinFormsTemplate.cs
│           |    ├── WorkerTemplate.cs
│           |    └── WpfTemplate.cs
|           ├── Python/
|           |    ├── DjangoTemplate.cs
|           |    ├── FastApiTemplate.cs
|           |    ├── FlaskTemplate.cs
|           |    └── PythonScriptTemplate.cs
│           ├── Node/
│           |    └── NodeTemplate.cs
│           └──DockerTemplate.cs
│ 
├── AuroCI.CLI/                 # UI layer — terminal interface
│   ├── Program.cs
│   └──Helpers/
│        ├── DirectoryNavigator.cs
│        └── DockerHelpers.cs 
│ 
├── AuroCI.Tests/               # Unit tests for core logic
    └── DtectorTests/
```

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Language | C# / .NET 10 |
| Terminal UI | Spectre.Console |
| Architecture | Interface-driven |
| File Parsing | System.IO |
| IDE | JetBrains Rider |

---

## 🖥️ Screenshots

**Main screen:**

<img width="544" height="448" alt="Main screen" src="https://github.com/user-attachments/assets/c529440b-99f8-4a2f-9714-ffcc00ded8aa" />

**Project browser:**

<img width="556" height="426" alt="Project browser" src="https://github.com/user-attachments/assets/f8d2bded-442b-4705-afae-fd4579063724" />

**Confirmation prompt:**

<img width="546" height="424" alt="Confirmation" src="https://github.com/user-attachments/assets/08ff2a41-a373-440c-8a32-e09e8f8242dd" />

---

## 🗺️ Roadmap

- [x] Smart project detection engine
- [x] Interactive CLI UI with Spectre.Console
- [x] ASP.NET Core Web template
- [x] .NET Console App template
- [x] .NET MAUI template (iOS/macOS & Android/Windows)
- [x] Avalonia UI template
- [x] WPF template
- [x] WinForms template
- [x] Blazor WASM template
- [x] Class Library template
- [x] Worker Service template
- [x] Global .NET Tool on NuGet (`dotnet tool install -g AuroCI`)
- [x] Manual template fallback for undetected projects
- [x] Dockerfile generation with .dockerignore support
- [x] Python support (Flask, Django, FastAPI, Data Science, Script)
- [x] Node.js support (Express, NestJS, Next.js, Angular, Vue, Script)
- [ ] Support for other languages (Go, Java)
- [ ] Custom template configuration via `auroci.json`

---

## 🔨 Build from Source

```bash
git clone https://github.com/d4nilx/AuroCI.git
cd AuroCI
dotnet run --project AuroCI.CLI
```

---

## 📄 License

Distributed under the MIT License. See [LICENSE](LICENSE) for more information.

---
![footer](https://capsule-render.vercel.app/api?section=footer&height=150&type=soft&color=0:0052D4,50:4364F7,100:6FB1FC&text=Built%20by%20d4nilx&fontSize=45&fontColor=ffffff)
