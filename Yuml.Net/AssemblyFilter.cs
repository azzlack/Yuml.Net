namespace Yuml.Net
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class AssemblyFilter
    {
        public IList<Type> Types { get; set; }

        public AssemblyFilter(Assembly assembly)
        {
            this.Types = new List<Type>(assembly.GetTypes());
        }
    }
}
