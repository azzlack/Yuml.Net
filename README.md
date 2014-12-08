Yuml.Net
========

Model diagram generator for .NET using [yUML.me](http://yUML.me)

Based on [Chris Owen's yuml-dotnet](https://github.com/chrisjowen/yuml-dotnet)

# How to use
Use the included `YumlFactory` or create your own implementation by inheriting the `IYumlFactory` interface.

#### Generating a class diagram with `YumlFactory` 
Pass your model classes into the constructor.
```csharp
var types = new List<Type>
                {
                    typeof(Person)
                };

var factory = new YumlFactory(types);
```

Then get the uri to the model diagram by calling the `GenerateClassDiagramUri` method.
```csharp
var imageUri = factory.GenerateClassDiagramUri();
```
    
To get public properties and methods, use the `DetailLevel` parameters:
```csharp
var imageUri = factory.GenerateClassDiagramUri(DetailLevel.PublicProperties, DetailLevel.PublicMethods);
// It is also possible to show private properties and methods 
// using the DetailLevel.PrivateProperties and DetailLevel.PrivateMethods enumerations.
```

NOTE: The uri's are cached for 30 000 seconds to enhance performance.
