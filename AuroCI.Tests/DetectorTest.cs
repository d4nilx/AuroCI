using AuroCI.Core.Detector;

namespace AuroCI.Tests;

// ===== Fixture =====
public class TempDirectoryFixture : IDisposable
{
    public string Path { get; } =
        System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());

    public TempDirectoryFixture() => Directory.CreateDirectory(Path);

    public void Dispose()
    {
        if (Directory.Exists(Path))
            Directory.Delete(Path, true);
    }
}

// ===== Tests =====
public class DetectorTest
{
    // .csproj — basic
    [Theory]
    [InlineData("<UseMaui>true</UseMaui>", "Maui")]
    [InlineData("Microsoft.NET.Sdk.Web", "Web")]
    [InlineData("<OutputType>Exe</OutputType>", "Console")]
    [InlineData("<OutputType>WinExe</OutputType>", "Console")]
    [InlineData("<UseWPF>true</UseWPF>", "WPF")]
    [InlineData("<UseWindowsForms>true</UseWindowsForms>", "WinForms")]
    [InlineData("Avalonia", "Avalonia")]
    [InlineData("Microsoft.AspNetCore.Components.WebAssembly", "BlazorWASM")]
    [InlineData("Sdk=\"Microsoft.NET.Sdk.Worker\"", "Worker")]
    [InlineData("<OutputType>Library</OutputType>", "Library")]
    public void Detect_CsprojContent_ReturnsCorrectType(string csprojContent, string expectedType)
    {
        using var fixture = new TempDirectoryFixture();
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "Test.csproj"),
            $"<Project><PropertyGroup>{csprojContent}</PropertyGroup></Project>");

        var result = new ProjectDetector().Detect(fixture.Path);

        Assert.Equal(expectedType, result.ProjectType);
    }

    // .csproj — edge cases
    [Fact]
    public void Detect_EmptyCsproj_ReturnsUnknown()
    {
        using var fixture = new TempDirectoryFixture();
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "Test.csproj"), "<Project/>");

        var result = new ProjectDetector().Detect(fixture.Path);

        Assert.Equal("Unknown", result.ProjectType);
    }

    [Fact]
    public void Detect_MultipleCsproj_DoesNotThrow()
    {
        using var fixture = new TempDirectoryFixture();
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "A.csproj"), "<Project/>");
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "B.csproj"), "<Project/>");

        var ex = Record.Exception(() => new ProjectDetector().Detect(fixture.Path));

        Assert.Null(ex);
    }

    // Python — basic
    [Theory]
    [InlineData("flask", "PythonFlask")]
    [InlineData("django", "PythonDjango")]
    [InlineData("fastapi", "PythonFastApi")]
    [InlineData("pandas", "PythonDataScience")]
    [InlineData("numpy", "PythonDataScience")]
    [InlineData("jupyter", "PythonDataScience")]
    [InlineData("requests", "PythonScript")]
    public void Detect_PythonRequirements_ReturnsCorrectType(string content, string expectedType)
    {
        using var fixture = new TempDirectoryFixture();
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "requirements.txt"), content);

        var result = new ProjectDetector().Detect(fixture.Path);

        Assert.Equal(expectedType, result.ProjectType);
    }

    // Python — edge cases
    [Fact]
    public void Detect_EmptyRequirementsTxt_ReturnsPythonScript()
    {
        using var fixture = new TempDirectoryFixture();
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "requirements.txt"), "");

        var result = new ProjectDetector().Detect(fixture.Path);

        Assert.Equal("PythonScript", result.ProjectType);
    }

    [Fact]
    public void Detect_PyprojectToml_WithFlask_ReturnsPythonFlask()
    {
        using var fixture = new TempDirectoryFixture();
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "pyproject.toml"), "flask");

        var result = new ProjectDetector().Detect(fixture.Path);

        Assert.Equal("PythonFlask", result.ProjectType);
    }

    [Fact]
    public void Detect_BothRequirementsAndPyproject_RequirementsTxtWins()
    {
        using var fixture = new TempDirectoryFixture();
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "requirements.txt"), "flask");
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "pyproject.toml"), "django");

        var result = new ProjectDetector().Detect(fixture.Path);

        Assert.Equal("PythonFlask", result.ProjectType);
    }

    // Node — basic
    [Theory]
    [InlineData("\"next\":", "NodeNext")]
    [InlineData("\"@angular/core\":", "NodeAngular")]
    [InlineData("\"vue\":", "NodeVue")]
    [InlineData("\"@nestjs/core\":", "NodeNest")]
    [InlineData("\"express\":", "NodeGeneral")]
    public void Detect_PackageJson_ReturnsCorrectType(string packageJsonContent, string expectedType)
    {
        using var fixture = new TempDirectoryFixture();
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "package.json"),
            $"{{ \"dependencies\": {{ {packageJsonContent} \"1.0.0\" }} }}");

        var result = new ProjectDetector().Detect(fixture.Path);

        Assert.Equal(expectedType, result.ProjectType);
    }

    // Node — edge cases
    [Fact]
    public void Detect_EmptyPackageJson_ReturnsNodeScript()
    {
        using var fixture = new TempDirectoryFixture();
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "package.json"), "{}");

        var result = new ProjectDetector().Detect(fixture.Path);

        Assert.Equal("NodeScript", result.ProjectType);
    }

    [Fact]
    public void Detect_InvalidJsonInPackageJson_DoesNotThrow()
    {
        using var fixture = new TempDirectoryFixture();
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "package.json"), "not json {{{{");

        var ex = Record.Exception(() => new ProjectDetector().Detect(fixture.Path));

        Assert.Null(ex);
    }

    [Fact]
    public void Detect_VueAndExpressInPackageJson_ReturnsNodeGeneral()
    {
        using var fixture = new TempDirectoryFixture();
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "package.json"),
            "{ \"dependencies\": { \"express\": \"1.0.0\", \"vue\": \"3.0.0\" } }");

        var result = new ProjectDetector().Detect(fixture.Path);

        Assert.Equal("NodeGeneral", result.ProjectType);
    }

    // General
    [Fact]
    public void Detect_EmptyDirectory_ReturnsUnknown()
    {
        using var fixture = new TempDirectoryFixture();

        var result = new ProjectDetector().Detect(fixture.Path);

        Assert.Equal("Unknown", result.ProjectType);
    }

    [Fact]
    public void Detect_NonExistentDirectory_ReturnsUnknown()
    {
        var result = new ProjectDetector().Detect("/this/does/not/exist");

        Assert.Equal("Unknown", result.ProjectType);
    }

    [Fact]
    public void Detect_EmptyStringPath_ReturnsUnknown()
    {
        var result = new ProjectDetector().Detect("");

        Assert.Equal("Unknown", result.ProjectType);
    }

    [Fact]
    public void Detect_PathWithSpaces_DetectsCorrectly()
    {
        using var fixture = new TempDirectoryFixture();
        File.WriteAllText(System.IO.Path.Combine(fixture.Path, "Test.csproj"),
            "<Project><PropertyGroup><UseMaui>true</UseMaui></PropertyGroup></Project>");

        var result = new ProjectDetector().Detect(fixture.Path);

        Assert.Equal("Maui", result.ProjectType);
    }
}