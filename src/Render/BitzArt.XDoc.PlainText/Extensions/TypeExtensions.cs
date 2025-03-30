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

    public static bool Has(this Type type, PropertyInfo property)
    {
        var properties = type.GetProperties();

        return properties.Any(p =>
            p.Name == property.Name &&
            p.PropertyType == property.PropertyType);
    }

    public static bool Has(this Type type, MethodInfo method)
    {
        var methods = type
            .GetMethods()
            .Where(m => m.Name == method.Name)
            .ToList();

        foreach (var candidate in methods)
        {
            if (IsSameSignature(candidate, method))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsSameSignature(MethodInfo a, MethodInfo b)
    {
        var paramsA = a.GetParameters();
        var paramsB = b.GetParameters();

        if (paramsA.Length != paramsB.Length)
        {
            return false;
        }

        // Compare return types and parameter types.
        if (a.ReturnType != b.ReturnType)
        {
            return false;
        }

        return paramsA
            .Select(p => p.ParameterType)
            .SequenceEqual(paramsB.Select(p => p.ParameterType));
    }

    public static bool Has(this Type type, FieldInfo field)
    {
        var fields = type.GetFields(
            BindingFlags.Instance |
            BindingFlags.Static |
            BindingFlags.Public |
            BindingFlags.NonPublic);

        return fields.Any(f =>
            f.Name == field.Name &&
            f.FieldType == field.FieldType);
    }
}