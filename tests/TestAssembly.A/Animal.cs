namespace TestAssembly.A;

public abstract class Animal
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Property of type <see cref="Cat"/> which we use for tests
    /// </summary>
    public Cat? Friend { get; set; }
}