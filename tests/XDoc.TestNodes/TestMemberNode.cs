using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;

namespace XDoc.Tests;

public class TestMemberNode
{
    private readonly TestNodeType _type;
    private readonly string _name;
    private readonly string _content;
    private readonly IReadOnlyCollection<ParameterInfo> _parameters;

    public TestMemberNode(Type type, string content)
        : this(TestNodeType.Type, type.FullName!, content) { }

    public TestMemberNode(FieldInfo field, string content)
        : this(TestNodeType.Field, GetMemberName(field), content) { }

    public TestMemberNode(PropertyInfo property, string content)
        : this(TestNodeType.Property, GetMemberName(property), content) { }

    public TestMemberNode(MethodInfo method, string content)
        : this(TestNodeType.Method, GetMemberName(method), content) =>
        _parameters = method.GetParameters();

    public TestMemberNode(TestNodeType type, string name, string content)
    {
        _type = type;
        _name = name;
        _content = content;
        _parameters = ImmutableList<ParameterInfo>.Empty;
    }

    public string GetXml()
    {
        var parameters = "";

        if (_type == TestNodeType.Method)
        {
            parameters = $"({string.Join(", ", _parameters.Select(Render))})";
        }

        return $"""
                <member name="{XmlNodeTypeChar}:{_name}{parameters}">
                    {_content.Offset(4, exceptFirstLine: true)}
                </member>
                """;
    }

    private static string Render(ParameterInfo p)
    {
        var type = p.ParameterType;
        var typeName = type.FullName ?? type.Name;

        // Remove the generic parameter count indicators from the type name
        // (e.g., List`1 becomes List)
        return typeName.Replace("`0", "").Replace("`", "");
    }

    private static string GetMemberName(MemberInfo member)
        => $"{member.DeclaringType!.FullName}.{member.Name}";

    private char XmlNodeTypeChar =>
        _type switch
        {
            TestNodeType.Type => 'T',
            TestNodeType.Field => 'F',
            TestNodeType.Property => 'P',
            TestNodeType.Method => 'M',
            _ => throw new UnreachableException()
        };
}