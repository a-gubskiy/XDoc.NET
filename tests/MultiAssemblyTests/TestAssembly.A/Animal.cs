namespace TestAssembly.A;

public abstract class BaseAnimal
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Animal color
    /// </summary>
    public abstract string Color { get; set; }
}

/// <summary>
/// Animal class
/// </summary>
public abstract class Animal : BaseAnimal, IAnimal
{
    /// <summary>
    /// Property of type <see cref="Cat"/> which we use for tests
    /// </summary>
    public Cat? Friend { get; set; }

    /// <summary>
    /// Description of Property1
    /// </summary>
    public virtual string Property1 { get; set; }

    /// <inheritdoc/>
    public override string Color { get; set; }
}