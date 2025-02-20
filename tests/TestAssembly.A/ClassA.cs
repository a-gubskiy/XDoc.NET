namespace TestAssembly.A;

/// <summary>
/// Test class A.
/// </summary>
public class ClassA : ClassBase
{
    /// <summary>
    /// Class A Name
    /// </summary>
    public virtual string Name { get; set; } = "";

    /// <summary>
    /// Return the name.
    /// </summary>
    /// <returns></returns>
    public virtual string GetName()
    {
        return "My name is " + Name;
    }
}