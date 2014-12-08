namespace Yuml.Net.Test.Models
{
    using System.Collections.Generic;

    using global::Yuml.Net.Test.Interfaces;

    public class Manager : User
    {
        public Resource<string> Department { get; set; }
    }
}