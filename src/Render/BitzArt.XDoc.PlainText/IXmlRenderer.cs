namespace BitzArt.XDoc;

public interface IXmlRenderer
{
    /// <summary>
    /// Renders the documentation of a <see cref="MemberDocumentation"/> as plain text.
    /// </summary>
    /// <param name="documentation"></param>
    /// <returns></returns>
    string Render(MemberDocumentation documentation);
}