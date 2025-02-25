namespace TestAssembly.A;

/// <inheritdoc />
public class BlackCat : Cat
{
    
    // public int Field1 { get; set; } //this property can cause System.Reflection.AmbiguousMatchException

    /// <summary>
    /// Field two
    /// </summary>
    public int Field2 { get; set; }

    /// <inheritdoc />
    public override string Name { get; set; } = "";

    /// <inheritdoc />
    public override string GetName()
    {
        return "H! My name is " + Name;
    }
}