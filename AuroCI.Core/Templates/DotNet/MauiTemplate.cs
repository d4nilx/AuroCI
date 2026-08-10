namespace AuroCI.Core.Templates.DotNet;

public class MauiTemplate : BaseTemplate
{
  public override string Name => "maui-ci.yml";

  protected override string GetYamlContent(string projectName)
    {
        return $@"name: {projectName} MAUI Multi-Platform CI/CD

on:
  push:
    branches: [ ""main"" ]
  pull_request:
    branches: [ ""main"" ]

jobs:
  # Job 1: Handles Android and Windows (Runs on Windows Host)
  build-android-windows:
    runs-on: windows-latest
    steps:
      - name: Checkout Code
        uses: actions/checkout@v4

      - name: Setup .NET SDK
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x' # Update to '10.0.x' if using MAUI 10

      - name: Install MAUI Workloads
        run: dotnet workload install maui

      # Optional Android Signing step would go here

      - name: Build Android App (AAB)
        run: dotnet publish -f net10.0-android -c Release

      - name: Build Windows App (MSIX)
        run: dotnet publish -f net10.0-windows10.0.19041.0 -c Release

      - name: Upload Android & Windows Artifacts
        uses: actions/upload-artifact@v4
        with:
          name: android-windows-packages
          path: |
            **/bin/Release/net10.0-android/*-Signed.aab
            **/bin/Release/net10.0-windows10.0.19041.0/win10-x64/AppPackages/**/*

  # Job 2: Handles iOS and macOS/MacCatalyst (Runs on macOS Host)
  build-ios-mac:
    runs-on: macos-latest
    steps:
      - name: Checkout Code
        uses: actions/checkout@v4

      - name: Setup .NET SDK
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'

      - name: Set Xcode Version
        uses: maxim-lobanov/setup-xcode@v1
        with:
          xcode-version: 'latest-stable'

      - name: Install MAUI Workloads
        run: dotnet workload install maui

      # Optional iOS Certificates & Provisioning Profiles import step would go here

      - name: Build iOS App (IPA)
        run: dotnet publish -f net10.0-ios -c Release /p:ArchiveOnBuild=true

      - name: Build macOS App (MacCatalyst app)
        run: dotnet publish -f net10.0-maccatalyst -c Release /p:ArchiveOnBuild=true

      - name: Upload Apple Artifacts
        uses: actions/upload-artifact@v4
        with:
          name: apple-packages
          path: |
            **/bin/Release/net10.0-ios/**/*.ipa
            **/bin/Release/net10.0-maccatalyst/**/*.pkg";
    }
}