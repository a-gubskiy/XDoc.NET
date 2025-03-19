namespace TestAssembly.A;

/// <inheritdoc/>
public class Cat : Animal
{
    /// <summary>
    /// Class A Name
    /// </summary>
    public virtual string Name { get; set; } = "";
    
    /// <summary>
    /// The wight of animal
    /// </summary>
    public virtual int Weight { get; set; }

    /// <summary>
    /// Return the name.
    /// </summary>
    /// <returns></returns>
    public virtual string GetName()
    {
        return "My name is " + Name;
    }
}