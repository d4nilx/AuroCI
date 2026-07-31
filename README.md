# ⚡ AuroCI

> A smart, interactive CLI tool for automated CI/CD pipeline generation — because writing YAML by hand is so 2020.

[![Platform](https://img.shields.io/badge/platform-macOS%20%7C%20Windows%20%7C%20Linux-32D74B?style=flat-square)](https://github.com/d4nilx/AuroCI)
[![.NET](https://img.shields.io/badge/.NET-10.0-00F2FF?style=flat-square)](https://dotnet.microsoft.com)

---

## 📌 About

**AuroCI** is a modern Command Line Interface (CLI) tool designed to eliminate the boilerplate of setting up GitHub Actions. It automatically scans your directory, detects the underlying .NET project type (MAUI, Web, or Console), and generates a fully configured, multi-platform CI/CD workflow instantly.

Built with an interface-driven architecture and a beautiful terminal UI, making DevOps automation accessible and visually pleasing.

### 🌟 Features

- 🔍 **Smart Auto-Detection** — parses `.csproj` files to identify your framework effortlessly.
- 🛠 **Zero Configuration** — generates complete `.github/workflows/` YAML files with one click.
- 🍎 **Cross-Platform CI/CD** — automatically configures multi-OS builds (e.g., Windows for Android, macOS for iOS in MAUI).
- 🎨 **Beautiful UI** — interactive terminal experience powered by Spectre.Console (no more boring text logs!).
- 🛡️ **Defensive Design** — safe file operations, interactive confirmations, and robust exception handling.

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| UI Framework | Spectre.Console |
| Language | C# |
| Architecture | Interface-driven (Factory Pattern) |
| File Parsing | System.IO / String manipulation |
| IDE | JetBrains Rider |

---

## 🚀 Quick Start

### Requirements

- [.NET SDK](https://dotnet.microsoft.com/download)
- A terminal (macOS Terminal, iTerm, Windows Terminal)

### Build & Run

```bash
git clone [https://github.com/d4nilx/AuroCI.git](https://github.com/d4nilx/AuroCI.git)
cd AuroCI
dotnet run --project AuroCI.CLI 
```

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
│       └── MauiTemplate.cs
│
└── AuroCI.CLI/                 # UI layer — terminal interface & commands
    ├── Commands/
    └── Program.cs
```

## 🗺️ Roadmap

- [x] Basic project detection engine
- [x] Interactive CLI UI with Spectre.Console
- [x] MAUI Template (iOS/macOS & Android/Windows runners)
- [x] ASP.NET Core Web App Template
- [x] .NET Console App Template
- [ ] Global .NET Tool installation support (dotnet tool install -g)
- [ ] Add support for the CI/CD pipeline for other languages (Node.js, Python, etc.)

## License
Distributed under the MIT License.

## ✉️ Contact

**Daniil Zdanov** — [@d4nilx](https://github.com/d4nilx)

Project: [github.com/d4nilx/AuroCI](https://github.com/d4nilx/AuroCI) 
