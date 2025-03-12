namespace TestAssembly.A;

public interface IAnimal
{
    /// <summary>
    /// This is property one.
    /// New line property description.
    /// </summary>
    string Property1 { get; set; }
    
    /// <summary>
    /// Return value of <c ref="Property1"/>.
    /// </summary>
    /// <returns></returns>
    string GetProperty1() => Property1;
}