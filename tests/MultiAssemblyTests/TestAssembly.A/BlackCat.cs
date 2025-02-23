namespace TestAssembly.A;

/// <inheritdoc />
public class BlackCat : Cat
{
    /// <summary>
    /// Field one
    /// </summary>
    public int Field1 { get; set; }

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