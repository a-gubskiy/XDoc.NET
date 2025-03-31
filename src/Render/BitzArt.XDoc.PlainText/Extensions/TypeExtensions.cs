using System.Reflection;

namespace BitzArt.XDoc;

/// <summary>
/// Contains extension methods for <see cref="Type"/>.
/// </summary>
internal static class TypeExtensions
{
    public static IEnumerable<Type> GetImmediateInterfaces(this Type type)
    {
        var inheritedInterfaces = type.GetInterfaces().SelectMany(i => i.GetInterfaces()).ToList();
        inheritedInterfaces.AddRange(type.BaseType?.GetInterfaces() ?? []);

        return type.GetInterfaces().Except(inheritedInterfaces);
    }
}