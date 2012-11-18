﻿namespace Yuml.Net
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using global::Yuml.Net.Interfaces;
    using global::Yuml.Net.Yuml;

    /// <summary>
    /// Factory for generating yUML diagrams
    /// </summary>
    public class YumlFactory : IYumlFactory
    {
        /// <summary>
        /// The base yUML URI
        /// </summary>
        private string baseUri = "http://yuml.me/diagram";

        /// <summary>
        /// Gets or sets a value indicating whether [first pass].
        /// </summary>
        /// <value><c>true</c> if [first pass]; otherwise, <c>false</c>.</value>
        private bool isFirstPass;

        /// <summary>
        /// The relationships
        /// </summary>
        private readonly IList<Relationship> relationships = new List<Relationship>();

        /// <summary>
        /// Gets or sets the types to generate diagrams for.
        /// </summary>
        /// <value>The types to generate diagrams for.</value>
        public IList<Type> Types { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YumlFactory" /> class.
        /// </summary>
        /// <param name="diagramType">Type of the diagram.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="scale">The scale.</param>
        public YumlFactory(DiagramType diagramType = DiagramType.Plain, Direction direction = Direction.LeftToRight, Scale scale = Scale.Normal)
        {
            var options = string.Empty;

            switch (diagramType)
            {
                case DiagramType.Boring:
                    options += "boring;";
                    break;
                case DiagramType.Plain:
                    options += "plain;";
                    break;
                case DiagramType.Scruffy:
                    options += "scruffy;";
                    break;
            }

            switch (direction)
            {
                case Direction.LeftToRight:
                    options += "dir:LR;";
                    break;
                case Direction.TopToBottom:
                    options += "dir:TD;";
                    break;
                case Direction.RightToLeft:
                    options += "dir:RL;";
                    break;
            }

            switch (scale)
            {
                case Scale.Huge:
                    options += "scale:180;";
                    break;
                case Scale.Big:
                    options += "scale:120;";
                    break;
                case Scale.Normal:
                    options += "scale:100;";
                    break;
                case Scale.Small:
                    options += "scale:80;";
                    break;
                case Scale.Tiny:
                    options += "scale:60;";
                    break;
            }

            this.baseUri += "/" + options;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YumlFactory" /> class.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="diagramType">Type of the diagram.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="scale">The scale.</param>
        public YumlFactory(
            IList<Type> types,
            DiagramType diagramType = DiagramType.Plain,
            Direction direction = Direction.LeftToRight,
            Scale scale = Scale.Normal)
            : this(diagramType, direction, scale)
        {
            this.Types = types;
        }

        /// <summary>
        /// Generates a class diagram.
        /// </summary>
        /// <param name="detailLevels">The detail levels.</param>
        /// <returns>The url to the class diagram.</returns>
        public string GenerateClassDiagram(params DetailLevel[] detailLevels)
        {
            this.baseUri += "/class/";

            var sb = new StringBuilder(baseUri);

            foreach (var type in this.Types)
            {
                if (this.Types.Contains(type) && type.IsClass)
                {
                    if (this.isFirstPass)
                    {
                        sb.Append(",");
                    }

                    // Get class header
                    sb.AppendFormat("[{0}{1}", this.GetInterfacesAsYuml(type), type.Name);

                    if (detailLevels.Contains(DetailLevel.PrivateProperties | DetailLevel.PublicProperties))
                    {
                        // Get class details (properties)
                        sb.AppendFormat("|{0}", this.GetClassPropertiesAsYuml(type, detailLevels));
                    }

                    if (detailLevels.Contains(DetailLevel.PrivateMethods | DetailLevel.PublicMethods))
                    {
                        // Get class details (methods)
                        sb.AppendFormat("|{0}", this.GetClassMethodsAsYuml(type, detailLevels));
                    }

                    sb.Append("]");

                    // Get derived classes
                    sb.Append(this.GetDerivedClassesAsYuml(type, detailLevels));

                    // Get associated classes
                    sb.Append(this.GetAssociatedClassesAsYuml(type, detailLevels));
                }

                if (!this.isFirstPass) 
                { 
                    this.isFirstPass = true;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the interfaces.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The interfaces in yUML format.</returns>
        private string GetInterfacesAsYuml(Type type)
        {
            var sb = new StringBuilder();

            foreach (var interfaceType in type.GetInterfaces())
            {
                if (this.Types.Contains(interfaceType)) 
                {
                    sb.AppendFormat("<<{0}>>;", interfaceType.Name);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the derived classes.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="detailLevels">The detail levels.</param>
        /// <returns>The derived classes in yUML format.</returns>
        private string GetDerivedClassesAsYuml(Type type, DetailLevel[] detailLevels)
        {
            var prevType = type;
            var sb = new StringBuilder();

            while (type.BaseType != null)
            {
                type = type.BaseType;

                if (this.Types.Contains(type))
                {
                    var relationship = new Relationship(prevType, type, RelationshipType.Inherits);

                    if (!this.relationships.Any(r => (r.Type1 == relationship.Type1 && r.Type2 == relationship.Type2 && r.RelationshipType == relationship.RelationshipType)))
                    {
                        sb.AppendFormat(",[{0}{1}]^-[{2}{3}", this.GetInterfacesAsYuml(prevType), prevType.Name, this.GetInterfacesAsYuml(type), type.Name);

                        if (detailLevels.Contains(DetailLevel.PrivateProperties | DetailLevel.PublicProperties))
                        {
                            // Get class details (properties)
                            sb.AppendFormat("|{0}", this.GetClassPropertiesAsYuml(type, detailLevels));
                        }

                        if (detailLevels.Contains(DetailLevel.PrivateMethods | DetailLevel.PublicMethods))
                        {
                            // Get class details (methods)
                            sb.AppendFormat("|{0}", this.GetClassMethodsAsYuml(type, detailLevels));
                        }

                        sb.Append("]");

                        this.relationships.Add(relationship);
                    }
                }

                prevType = type;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the assosiated classes.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="detailLevels">The detail levels.</param>
        /// <returns>The associated classes in yUML format.</returns>
        private string GetAssociatedClassesAsYuml(Type type, DetailLevel[] detailLevels)
        {
            var sb = new StringBuilder();

            foreach (var property in type.GetProperties())
            {

                if (this.Types.Contains(property.PropertyType))
                {
                    sb.AppendFormat(
                        ",[{0}{1}]->[{2}{3}",
                        this.GetInterfacesAsYuml(type),
                        type.Name,
                        this.GetInterfacesAsYuml(property.PropertyType),
                        property.PropertyType.Name);

                    if (detailLevels.Contains(DetailLevel.PrivateProperties | DetailLevel.PublicProperties))
                    {
                        // Get class details (properties)
                        sb.AppendFormat("|{0}", this.GetClassPropertiesAsYuml(type, detailLevels));
                    }

                    if (detailLevels.Contains(DetailLevel.PrivateMethods | DetailLevel.PublicMethods))
                    {
                        // Get class details (methods)
                        sb.AppendFormat("|{0}", this.GetClassMethodsAsYuml(type, detailLevels));
                    }

                    sb.Append("]");
                }
                else if (property.PropertyType.IsGenericType)
                {
                    var isEnumerable = property.PropertyType.GetInterface(typeof(IEnumerable).FullName) != null;
                    var typeParameters = property.PropertyType.GetGenericArguments();

                    if (this.Types.Contains(typeParameters[0]) && isEnumerable)
                    {
                        sb.AppendFormat(",[{0}{1}]1-0..*[{2}{3}", this.GetInterfacesAsYuml(type), type.Name, this.GetInterfacesAsYuml(typeParameters[0]), typeParameters[0].Name);

                        if (detailLevels.Contains(DetailLevel.PrivateProperties | DetailLevel.PublicProperties))
                        {
                            // Get class details (properties)
                            sb.AppendFormat("|{0}", this.GetClassPropertiesAsYuml(type, detailLevels));
                        }

                        if (detailLevels.Contains(DetailLevel.PrivateMethods | DetailLevel.PublicMethods))
                        {
                            // Get class details (methods)
                            sb.AppendFormat("|{0}", this.GetClassMethodsAsYuml(type, detailLevels));
                        }

                        sb.Append("]");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the class properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="detailLevels">The detail levels.</param>
        /// <returns>The class properties in yUML format.</returns>
        private string GetClassPropertiesAsYuml(Type type, DetailLevel[] detailLevels)
        {
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic).OrderBy(x => x.GetSetMethod() == null).ThenBy(x => x.Name);

            var sb = new StringBuilder();

            foreach (var propertyInfo in properties)
            {
                // Private properties
                if (detailLevels.Contains(DetailLevel.PrivateProperties) && propertyInfo.GetSetMethod() == null)
                {
                    sb.AppendFormat("- {0} : {1};", propertyInfo.Name, this.YumlEncode(propertyInfo.PropertyType.ToString()));
                }

                // Public properties
                if (detailLevels.Contains(DetailLevel.PublicProperties) && propertyInfo.GetSetMethod() != null)
                {
                    sb.AppendFormat("+ {0} : {1};", propertyInfo.Name, this.YumlEncode(propertyInfo.PropertyType.ToString()));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the class methods.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="detailLevels">The detail levels.</param>
        /// <returns>The class properties in yUML format.</returns>
        private string GetClassMethodsAsYuml(Type type, DetailLevel[] detailLevels)
        {
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic).Where(m => !m.IsSpecialName).OrderBy(x => x.IsPrivate).ThenBy(x => x.Name);

            var sb = new StringBuilder();

            foreach (var methodInfo in methods)
            {
                // Private methods
                if (detailLevels.Contains(DetailLevel.PrivateMethods) && methodInfo.IsPrivate)
                {
                    sb.AppendFormat("- {0}();", methodInfo.Name);
                }

                // Public methods
                if (detailLevels.Contains(DetailLevel.PublicMethods) && methodInfo.IsPublic)
                {
                    sb.AppendFormat("+ {0}();", methodInfo.Name);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Encodes the string to yUML format
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>A yUML encoded string.</returns>
        private string YumlEncode(string str)
        {
            var rstr = str;

            if (rstr.StartsWith("System."))
            {
                rstr = rstr.Substring(7);
            }

            rstr = rstr
                .Replace("`1", string.Empty)
                .Replace('[', '［')
                .Replace(']', '］')
                .Replace('#', '＃');

            return rstr;
        }
    }
}