# xdoc.net

A lightweight and efficient tool for parsing, and managing C# XML documentation comments.


### BitzArt.XDoc

BitzArt.XDoc is a lightweight .NET library for parsing and accessing XML documentation comments from your C# code. It provides an intuitive API to retrieve documentation for:
* Types/classes
* Properties
* Methods
* Fields


```csharp

using BitzArt.XDoc;
...

var xdoc = new XDoc();

var typeDocs = xdoc.Get(typeof(MyType));
var propertyDocs = xDoc.Get(typeof(MyType).GetProperty(nameof(MyType.PropertyOne)));
```

### BitzArt.XDoc.PlainText

BitzArt.XDoc.PlainText is an extension package for BitzArt.XDoc that enables rendering XML documentation comments into plain text.

```csharp

using BitzArt.XDoc;
using Xdoc.Renderer.PlaintText;

...
    
var xdoc = new XDoc();

var typeDescrption = xDoc.Get(typeof(MyType)).ToPlainText();
var propertyDescrption = xDoc.Get(typeof(MyType).GetProperty(nameof(MyType.PropertyOne))).ToPlainText();

```


### BitzArt.XDoc.EntityFrameworkCore

BitzArt.XDoc.EntityFrameworkCore is an extension library that bridges XML documentation comments from C# code to Entity Framework Core database objects.

```csharp

using BitzArt.XDoc;
using Xdoc.Renderer.PlaintText;
using BitzArt.XDoc.EntityFrameworkCore
...
    

// TODO: Add example

```