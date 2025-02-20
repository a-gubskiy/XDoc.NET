using TestAssembly.A;

namespace TestAssembly.B;

/// <summary>
/// Test class B.
/// </summary>
public class ClassB : ClassBase
{
    /// <summary>
    /// Class B Name
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Field one
    /// </summary>
    public string Field1 { get; set; } = "";

    /// <summary>
    /// Field two
    /// </summary>
    public string Field2 { get; set; } = "";
}