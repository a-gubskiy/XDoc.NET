namespace TestAssembly.A;

public class ClassBase
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Property of type <see cref="ClassA"/> which we use for tests
    /// </summary>
    public ClassA? ClassBaseProperty { get; set; }
}