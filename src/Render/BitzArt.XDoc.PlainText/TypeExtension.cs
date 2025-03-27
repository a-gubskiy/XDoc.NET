namespace BitzArt.XDoc;

/// <summary>
/// Contains extension methods for <see cref="Type"/>.
/// </summary>
public static class TypeExtension
{
    /// <summary>
    /// Returns all parent types of the specified type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IReadOnlyCollection<Type> GetParents(this Type type)
    {
        var result = new List<Type>();

        if (type.BaseType != null)
        {
            result.Add(type.BaseType);
        }

        result.AddRange(type.GetInterfaces());

        return result.AsReadOnly();
    }
}