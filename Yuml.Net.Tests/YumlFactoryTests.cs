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
        public void GenerateClassDiagram_WheGivenSingleClass_ShouldReturnDiagramUrl()
        {
            var types = new List<Type>
                            {
                                typeof(Person)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[Person]", new YumlFactory(types).GenerateClassDiagram());
        }

        [Test]
        public void GenerateClassDiagram_WheGivenInheritedClassWithBase_ShouldReturnDiagramUrlWithBase()
        {
            var types = new List<Type>
                            {
                                typeof(User), 
                                typeof(Person)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[User],[User]^-[Person],[Person]", new YumlFactory(types).GenerateClassDiagram());
        }

        [Test]
        public void GenerateClassDiagram_WhenGivenInheritedClassWithoutBase_ShouldReturnDiagramUrlWithoutBase()
        {
            var types = new List<Type>
                            {
                                typeof(User)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[User]", new YumlFactory(types).GenerateClassDiagram());
        }

        [Test]
        public void GenerateClassDiagram_WhenGivenMultipleInheritedClasses_ShouldReturnDiagramUrlWithAllInheritances()
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
        public void GenerateClassDiagram_WhenGivenClassWithInterface_ShouldReturnDiagramUrl()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(IAdministrator)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[<<IAdministrator>>;Administrator]", new YumlFactory(types).GenerateClassDiagram());
        }

        [Test]
        public void GenerateClassDiagram_WhenGivenClassWithClassProperty_ShouldReturnDiagramUrlWithAssociation()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(Domain)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[Administrator],[Administrator]->[Domain],[Domain]", new YumlFactory(types).GenerateClassDiagram());
        }

        [Test]
        public void GenerateClassDiagram_WhenGivednClassWithListOfClassProperty_ShouldReturnDiagramUrlWithManyAssociation()
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
        public void GenerateClassDiagram_WhenGivenDetailedClassesWithListOfClassProperty_ShouldReturnDetailedDiagramUrlWithManyAssociation()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(Role), 
                                typeof(Domain)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[Administrator|+ Domain : Domain;+ Roles : IList<Role>|+ ChangePassword()],[Administrator]->[Domain|+ Name : string;+ Uri : string],[Administrator]1-0..*[Role|+ Name : string],[Role|+ Name : string],[Domain|+ Name : string;+ Uri : string]", new YumlFactory(types).GenerateClassDiagram(DetailLevel.PublicProperties, DetailLevel.PublicMethods));
        }

        [Test]
        public void GenerateClassDiagram_WhenGivenManyDetailedClassesWithListOfClassProperty_ShouldReturnDetailedDiagramUrlWithManyAssociation()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(IAdministrator),
                                typeof(User), 
                                typeof(Person), 
                                typeof(Role), 
                                typeof(Domain)
                            };

            Assert.AreEqual("http://yuml.me/diagram/plain;dir:LR;scale:100;/class/[<<IAdministrator>>;Administrator|+ Domain : Domain;+ Roles : IList<Role>|+ ChangePassword()],[<<IAdministrator>>;Administrator]^-[User|+ Password : string;+ Username : string],[User]^-[Person|+ Name : string],[<<IAdministrator>>;Administrator]->[Domain|+ Name : string;+ Uri : string],[<<IAdministrator>>;Administrator]1-0..*[Role|+ Name : string],[User|+ Password : string;+ Username : string],[Person|+ Name : string],[Role|+ Name : string],[Domain|+ Name : string;+ Uri : string]", new YumlFactory(types).GenerateClassDiagram(DetailLevel.PrivateProperties, DetailLevel.PublicProperties, DetailLevel.PrivateMethods, DetailLevel.PublicMethods));
        }
    }
}
