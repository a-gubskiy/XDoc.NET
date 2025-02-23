using Xdoc.Models;

namespace Xdoc.Abstractions;

public interface IAssemblyXmlInfo
{
    /// <summary>
    /// Assembly name.
    /// </summary>
    string Name { get; init; }

    /// <summary>
    /// Try to find information about a class.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    ClassXmlInfo? GetClassInfo(Type type);
}