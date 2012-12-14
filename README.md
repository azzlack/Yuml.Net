Yuml.Net
========

Model diagram generator for .NET using [yUML.me](http://yUML.me)

Based on [Chris Owen's yuml-dotnet](https://github.com/chrisjowen/yuml-dotnet)

# How to use
Use the included `YumlFactory` or create your own implementation by inheriting the `IYumlFactory` interface.

#### Using `YumlFactory` 
Pass your model classes into the constructor.

    var types = new List<Type>
                    {
                        typeof(Person)
                    };

    var factory = new YumlFactory(types);
    
Then get the uri to the model diagram by calling the `GenerateClassDiagramUri` method.

    var imageUri = factory.GenerateClassDiagramUri();
    
NOTE: The uri's are cached for 30 000 seconds enhance performance.
