namespace Yuml.Net.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for the yUML factory
    /// </summary>
    public interface IYumlFactory
    {
        /// <summary>
        /// Gets or sets the types to generate diagrams for.
        /// </summary>
        /// <value>The types to generate diagrams for.</value>
        IList<Type> Types { get; set; }

        /// <summary>
        /// Generates a class diagram uri.
        /// </summary>
        /// <param name="detailLevels">The detail levels.</param>
        /// <returns>The url to the class diagram.</returns>
        Task<string> GenerateClassDiagramUri(params DetailLevel[] detailLevels);
    }
}