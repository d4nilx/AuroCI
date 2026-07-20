namespace AuroCI.Core.Interfaces;

public interface ITemplateGenerator
{
    // each generator need to have a name to print it in the menu
    string Name { get; }
    
    // Here it makes directories and files for the project 
    void Generate(string projectName, string targetDirectory);
}