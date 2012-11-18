namespace Yuml.Net.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the type name in yUML format
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A yUML encoded string.</returns>
        public static string GetYumlName(this Type type)
        {
            var rstr = type.GetFriendlyName();

            rstr = rstr
                .Replace("<<", "«")
                .Replace(">>", "»")
                .Replace('[', '［')
                .Replace(']', '］')
                .Replace('#', '＃');

            return rstr;
        }

        /// <summary>
        /// Gets the friendly name of the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        public static string GetFriendlyName(this Type type)
        {
            if (type == typeof(int)) 
            { 
                return "int";
            }

            if (type == typeof(string)) 
            { 
                return "string";
            }

            var result = GetFriendlyTypeName(type);
            
            if (type.IsGenericType) 
            { 
                result = result + GetFriendlyNameForGeneric(type.GetGenericArguments());
            }

            return result;
        }

        /// <summary>
        /// Pretties the name of the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        private static string GetFriendlyTypeName(Type type)
        {
            var result = type.Name;
            
            if (type.IsGenericType) 
            {
                result = result.Remove(result.IndexOf('`'));
            }

            if (type.IsNested && !type.IsGenericParameter) 
            { 
                return type.DeclaringType.GetFriendlyName() + "." + result;
            }

            return result;
        }

        /// <summary>
        /// Gets the friendly name for the generic type.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns>The generic type name.</returns>
        private static string GetFriendlyNameForGeneric(IEnumerable<Type> types)
        {
            var result = "";
            var delim = "<";

            foreach (var t in types)
            {
                result += delim;
                delim = ",";
                result += t.GetFriendlyName();
            }

            return result + ">";
        }
    }
}