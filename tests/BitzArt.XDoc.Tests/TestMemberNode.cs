using System.Diagnostics;
using System.Reflection;

namespace BitzArt.XDoc.Tests;

public class TestMemberNode
{
    private readonly TestNodeType _type;
    private readonly string _name;
    private readonly string _content;

    public TestMemberNode(Type type, string content)
        : this(TestNodeType.Type, type.FullName!, content) { }

    public TestMemberNode(FieldInfo field, string content)
        : this(TestNodeType.Field, GetMemberName(field), content) { }

    public TestMemberNode(PropertyInfo property, string content)
        : this(TestNodeType.Property, GetMemberName(property), content) { }

    public TestMemberNode(MethodInfo method, string content)
        : this(TestNodeType.Method, GetMemberName(method), content) { }

    public TestMemberNode(TestNodeType type, string name, string content)
    {
        _type = type;
        _name = name;
        _content = content;
    }

    public string GetXml() =>
        $"""
        <member name="{XmlNodeTypeChar}:{_name}">
            {_content.Offset(4, exceptFirstLine: true)}
        </member>
        """;

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

public enum TestNodeType
{
    Type,
    Field,
    Property,
    Method
}

public static class TestXmlNodesExtensions
{
    public static string GetXml(this IEnumerable<TestMemberNode> nodes, Assembly assembly) =>
        $"""
        <?xml version="1.0"?>
        <doc>
            <assembly>
                <name>{assembly.GetName().Name}</name>
            </assembly>
            <members>
                {string.Join('\n', nodes.Select(node => node.GetXml().Offset(8, exceptFirstLine: true)))}
            </members>
        </doc>
        """;
}

public static class StringExtensions
{
    // Offsets the content by the specified number of spaces.
    // In case if the content has multiple lines, each line will be offset.
    public static string Offset(this string content, int offset, bool exceptFirstLine = false)
    {
        var lines = content.Split('\n');

        var offsetString = new string(' ', offset);

        for (var i = exceptFirstLine ? 1 : 0; i < lines.Length; i++)
            lines[i] = $"{offsetString}{lines[i]}";

        return string.Join('\n', lines);
    }
}