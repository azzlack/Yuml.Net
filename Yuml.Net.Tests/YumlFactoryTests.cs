namespace Yuml.Net.Test
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using global::Yuml.Net.Test.Interfaces;
    using global::Yuml.Net.Test.Models;

    [TestFixture]
    public class YumlFactoryTests
    {
        [Test]
        public void Can_Generate_Single_Class_Diagram()
        {
            var types = new List<Type>
                            {
                                typeof(Person)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[Person]", new YumlFactory(types).GenerateClassDiagram());
        }

        [Test]
        public void Can_Generate_Inherited_Class_Diagram()
        {
            var types = new List<Type>
                            {
                                typeof(User), 
                                typeof(Person)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[User],[User]^-[Person],[Person]", new YumlFactory(types).GenerateClassDiagram());
        }

        [Test]
        public void Will_Not_Generate_Inherited_Class_Diagram_If_Not_In_List()
        {
            var types = new List<Type>
                            {
                                typeof(User)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[User]", new YumlFactory(types).GenerateClassDiagram());
        }

        [Test]
        public void Can_Generate_Inherited_Class_Diagram_To_Several_Layers()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(User), 
                                typeof(Person)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[Administrator],[Administrator]^-[User],[User]^-[Person],[User],[Person]", new YumlFactory(types).GenerateClassDiagram());
        }

        [Test]
        public void Can_Generate_Class_With_Interfaces()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(IAdministrator)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[<<IAdministrator>>;Administrator]", new YumlFactory(types).GenerateClassDiagram());
        }

        [Test]
        public void Can_Generate_Class_With_Association()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(Domain)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[Administrator],[Administrator]->[Domain],[Domain]", new YumlFactory(types).GenerateClassDiagram());
        }

        [Test]
        public void Can_Generate_Class_With_A_Many_Association()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(Role), 
                                typeof(Domain)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[Administrator],[Administrator]->[Domain],[Administrator]1-0..*[Role],[Role],[Domain]", new YumlFactory(types).GenerateClassDiagram());
        }

        [Test]
        public void Can_Generate_Detailed_Class_With_A_Many_Association()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(Role), 
                                typeof(Domain)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[Administrator|+ Domain : Yuml.Net.Test.Models.Domain;+ Roles : Collections.Generic.IList［Yuml.Net.Test.Models.Role］;|+ ChangePassword();],[Administrator]->[Domain|+ Domain : Yuml.Net.Test.Models.Domain;+ Roles : Collections.Generic.IList［Yuml.Net.Test.Models.Role］;|+ ChangePassword();],[Administrator]1-0..*[Role|+ Domain : Yuml.Net.Test.Models.Domain;+ Roles : Collections.Generic.IList［Yuml.Net.Test.Models.Role］;|+ ChangePassword();],[Role|+ Name : String;|],[Domain|+ Name : String;+ Uri : String;|]", new YumlFactory(types).GenerateClassDiagram(DetailLevel.PublicProperties, DetailLevel.PublicMethods));
        }
    }
}
