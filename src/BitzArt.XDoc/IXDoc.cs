using System.Reflection;

namespace BitzArt.XDoc;

public interface IXDoc
{
    /// <summary>
    /// Fetches documentation for the specified <see cref="Assembly"/>.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/> to retrieve documentation for.</param>"/>
    /// <returns><see cref="AssemblyDocumentation"/> for the specified <see cref="Assembly"/>.</returns>
    AssemblyDocumentation? Get(Assembly assembly);

    /// <summary>
    /// Fetches documentation for the specified <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to retrieve documentation for.</param>
    /// "/>
    /// <returns>
    /// <see cref="TypeDocumentation"/> for the specified <see cref="Type"/> if available;
    /// otherwise, <see langword="null"/>.
    /// </returns>
    TypeDocumentation? Get(Type? type);

    /// <summary>
    /// Fetches documentation for the specified <see cref="PropertyInfo"/>.
    /// </summary>
    /// <param name="property">The <see cref="PropertyInfo"/> to retrieve documentation for.</param>
    /// <returns>
    /// <see cref="PropertyDocumentation"/> for the specified <see cref="PropertyInfo"/> if available;
    /// otherwise, <see langword="null"/>.
    /// </returns>
    PropertyDocumentation? Get(PropertyInfo? property);

    /// <summary>
    /// Fetches documentation for the specified <see cref="MethodInfo"/>.
    /// </summary>
    /// <param name="methodInfo">
    /// The <see cref="MethodInfo"/> to retrieve documentation for.
    /// </param>
    /// <returns>
    /// <see cref="MethodDocumentation"/> for the specified <see cref="MethodInfo"/> if available;
    /// otherwise, <see langword="null"/>.
    /// </returns>
    /// <exception cref="NotImplementedException"></exception>
    MethodDocumentation? Get(MethodInfo? methodInfo);

    /// <summary>
    /// Fetches documentation for the specified <see cref="FieldInfo"/>.
    /// </summary>
    /// <param name="fieldInfo">
    /// The <see cref="FieldInfo"/> to retrieve documentation for.
    /// </param>
    /// <returns>
    /// <see cref="FieldDocumentation"/> for the specified <see cref="FieldInfo"/> if available;
    /// otherwise, <see langword="null"/>.
    /// </returns>
    /// <exception cref="NotImplementedException"></exception>
    FieldDocumentation? Get(FieldInfo? fieldInfo);
}