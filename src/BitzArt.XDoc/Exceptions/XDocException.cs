namespace BitzArt.XDoc;

/// <summary>
/// An exception thrown when an error occurs in the XDoc library.
/// </summary>
public class XDocException : Exception
{
    // Internal constructor ensures that this exception can only be thrown from within the library.
    // The class itself is public though, allowing for catching in user code, if needed.
    internal XDocException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}