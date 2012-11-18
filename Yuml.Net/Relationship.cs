namespace Yuml.Net
{
    using System;

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
}