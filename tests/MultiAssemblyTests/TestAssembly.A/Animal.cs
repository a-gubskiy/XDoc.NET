namespace TestAssembly.A;

public abstract class BaseAnimal
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public int Id { get; set; }
}

public abstract class Animal : BaseAnimal, IAnimal
{
    /// <summary>
    /// Property of type <see cref="Cat"/> which we use for tests
    /// </summary>
    public Cat? Friend { get; set; }

    public string Property1 { get; set; }
}