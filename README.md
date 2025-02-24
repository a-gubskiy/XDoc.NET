# xdoc.net

A lightweight and efficient tool for parsing, and managing C# XML documentation comments.


### Proposed usage example

```csharp

using BitzArt.XDoc;

...

var type = typeof(MyType);

var xdoc = new XDoc();

var typeDocs = xdoc.Get(type);

var typeComments = typeDocs.ToPlainText(); // Fully resolved type commens (include inherited comments and crefs)
```

### Notes for internal implementation
```csharp

var xml = typeDocs.Node; //Contains the original XML node from .NET generated xml file
var parsedContent = typeDocs.ParsedContent; // Triggers Resolve() if not previously resolved


var a = parsedContent.OriginalNode;
var b = parsedContent.References; // IEnumerable<ParsedContent>

typeDocs._isResolved = false; // => typeDocs.Resolve();

```
