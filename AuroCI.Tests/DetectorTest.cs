using AuroCI.Core.Detector;

namespace AuroCI.Tests;

public class DetectorTest
{
    [Theory]
    [InlineData("<UseMaui>true</UseMaui>", "Maui")]
    [InlineData("Microsoft.NET.Sdk.Web", "Web")]
    [InlineData("<OutputType>Exe</OutputType>", "Console")]
    [InlineData("<UseWPF>true</UseWPF>", "WPF")]
    [InlineData("<UseWindowsForms>true</UseWindowsForms>", "WinForms")]
    [InlineData("Avalonia", "Avalonia")]
    [InlineData("Microsoft.AspNetCore.Components.WebAssembly", "BlazorWASM")]
    public void Detect_ProjectType_ReturnsCorrectType(string csprojContent, string expectedType)
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        File.WriteAllText(Path.Combine(tempDir, "Test.csproj"), 
            $"<Project><PropertyGroup>{csprojContent}</PropertyGroup></Project>");
        var detect = new ProjectDetector();
        
        // Act
        var result = detect.Detect(tempDir);

        //Asset
        Assert.Equal(expectedType, result.ProjectType);
        
        // Cleaning 
        Directory.Delete(tempDir, true);
    }
    
    [Theory]
    [InlineData("flask", "PythonFlask")]
    [InlineData("django", "PythonDjango")]
    [InlineData("fastapi", "PythonFastApi")]
    [InlineData("pandas", "PythonDataScience")]
    [InlineData("numpy", "PythonDataScience")]
    [InlineData("requests", "PythonScript")]
    public void Detect_ProjectType_PythonRequirements_ReturnsCorrectType(string requirementsContent, string expectedType)
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        File.WriteAllText(Path.Combine(tempDir, "requirements.txt"), requirementsContent);
        var detect = new ProjectDetector();
        
        // Act
        var result = detect.Detect(tempDir);

        //Asset
        Assert.Equal(expectedType, result.ProjectType);
        
        // Cleaning 
        Directory.Delete(tempDir, true);
    }

    [Fact]
    public void Detect_ProjectType_DoesNotExist()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        
        var result = new ProjectDetector().Detect(tempDir);
        
        Assert.Equal("Unknown", result.ProjectType);
        Directory.Delete(tempDir, true);
    }
    [Fact]
    public void Detect_NonExistentDirectory_ReturnsUnknown()
    {
        var result = new ProjectDetector().Detect("/this/does/not/exist");
        Assert.Equal("Unknown", result.ProjectType);
    }
}
