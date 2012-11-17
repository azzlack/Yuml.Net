namespace Yuml.Net
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    public class YumlGenerator
    {
        const string BASEURI = "http://yuml.me/diagram/scruffy/class/";
        public IList<Type> Types { get; set; }
        public bool FirstPass { get; set; }
        public List<Relationship> Relationships = new List<Relationship>();

        public YumlGenerator(IList<Type> Types)
        {
            this.FirstPass = false;
            this.Types = Types;
        }

        public string Yuml()
        {
            var sb = new StringBuilder(BASEURI);
            foreach (var type in this.Types)
            {
                if (!this.Types.Contains(type)) continue;
                if (type.IsClass)
                {
                    if (this.FirstPass) sb.Append(",");
                    sb.AppendFormat("[{0}{1}]", this.Interfaces(type), type.Name);
                    sb.Append(this.DerivedClasses(type));
                    sb.Append(this.AssosiatedClasses(type));
                }
                if (!this.FirstPass) this.FirstPass = true;
            }
            return sb.ToString();
        }



        private string Interfaces(Type type)
        {
            var sb = new StringBuilder();
            foreach (var interfaceType in type.GetInterfaces())
            {
                if (!this.Types.Contains(interfaceType)) continue;
                sb.AppendFormat("<<{0}>>;", interfaceType.Name);
            }
            return sb.ToString();
        }

        private string DerivedClasses(Type type)
        {
            var prevType = type;
            var sb = new StringBuilder();

            while (type.BaseType != null)
            {
                type = type.BaseType;
                if (this.Types.Contains(type))
                {
                    var relationship = new Relationship(prevType, type, RelationshipType.Inherits);

                    if (!this.Relationships.Exists(r => (r.Type1 == relationship.Type1 && r.Type2 == relationship.Type2 && r.RelationshipType == relationship.RelationshipType)))
                    {
                        sb.AppendFormat(",[{0}{1}]^-[{2}{3}]", this.Interfaces(prevType), prevType.Name, this.Interfaces(type), type.Name);
                        this.Relationships.Add(relationship);
                    }
                }
                prevType = type;
            }
            return sb.ToString();
        }

        private string AssosiatedClasses(Type type)
        {
            var sb = new StringBuilder();
            foreach (var property in type.GetProperties())
            {

                if (this.Types.Contains(property.PropertyType))
                {
                    sb.AppendFormat(",[{0}{1}]->[{2}{3}]", this.Interfaces(type), type.Name,
                                    this.Interfaces(property.PropertyType), property.PropertyType.Name);


                }
                else if (property.PropertyType.IsGenericType)
                {
                    var IsEnumerable = property.PropertyType.GetInterface(typeof(IEnumerable).FullName) != null;
                    var typeParameters = property.PropertyType.GetGenericArguments();

                    if (this.Types.Contains(typeParameters[0]) && IsEnumerable){
                        sb.AppendFormat(",[{0}{1}]1-0..*[{2}{3}]", this.Interfaces(type), type.Name,
                        this.Interfaces(typeParameters[0]), typeParameters[0].Name);
                    }
                }

                //if (type != property.PropertyType)
                //    AssosiatedClasses(property.PropertyType);

            }
            return sb.ToString();
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

    }

    public class Relationship
    {
        public Type Type1 { get; set; }
        public Type Type2 { get; set; }
        public RelationshipType RelationshipType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public Relationship(Type type1, Type type2, RelationshipType relationshipType)
        {
            this.Type1 = type1;
            this.Type2 = type2;
            this.RelationshipType = relationshipType;
        }
    }

    public enum RelationshipType
    {
        Inherits = 1,
        HasOne = 2
    }
}
