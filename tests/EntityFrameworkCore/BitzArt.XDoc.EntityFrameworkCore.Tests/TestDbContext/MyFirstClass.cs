using System.ComponentModel.DataAnnotations.Schema;

namespace BitzArt.XDoc.Tests;

public class MyFirstClass
{
    public const string IdComment = "Id Comment";
    
    public const string NullableIdComment = "Nullable Id Comment";

    /// <summary>
    /// Id Comment
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nullable Id Comment
    /// </summary>
    public int? NullableId { get; set; }
}
