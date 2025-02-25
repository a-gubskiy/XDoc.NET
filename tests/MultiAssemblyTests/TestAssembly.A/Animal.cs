namespace TestAssembly.A;

public abstract class Animal : IAnimal
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Property of type <see cref="Cat"/> which we use for tests
    /// </summary>
    public Cat? Friend { get; set; }

    public string Field1 { get; set; }
}