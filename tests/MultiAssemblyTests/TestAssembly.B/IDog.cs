using TestAssembly.A;

namespace TestAssembly.B;

/// <summary>
/// Defines a dog.
/// This interface include definition of two fields.
/// </summary>
public interface IDog : IAnimal
{
    /// <summary>
    /// This is field two
    /// New line field description.
    /// </summary>
    string Field2 { get; set; }
}