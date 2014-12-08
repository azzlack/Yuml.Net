namespace Yuml.Net.Test
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Mime;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using global::Yuml.Net.Test.Interfaces;

    using global::Yuml.Net.Test.Models;

    [TestFixture]
    public class YumlFactoryTests
    {
        private HttpClient client;

        [SetUp]
        public async void SetUp()
        {
            this.client = new HttpClient()
                              {
                                  BaseAddress = new Uri("http://yuml.me")
                              };
        }

        [Test]
        public async void GenerateClassDiagram_WheGivenSingleClass_ShouldReturnDiagramUrl()
        {
            // Get image uri
            var imageUri = await YumlFactory.Create<Person>().GenerateClassDiagramUri();

            // Verify that the uri is actually an image
            var response = await this.client.GetAsync(imageUri);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Image URI: {0}", imageUri.Replace("http://yuml.me/", "http://yuml.me/edit/"));

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(response.Content.Headers.ContentType.MediaType == "image/png");
        }

        [Test]
        public async void GenerateClassDiagram_WheGivenSingleClassWithGenericProperty_ShouldReturnDiagramUrl()
        {
            // Get image uri
            var imageUri = await YumlFactory.Create<Manager>().GenerateClassDiagramUri(DetailLevel.PublicProperties);

            // Verify that the uri is actually an image
            var response = await this.client.GetAsync(imageUri);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Image URI: {0}", imageUri.Replace("http://yuml.me/", "http://yuml.me/edit/"));

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(response.Content.Headers.ContentType.MediaType == "image/png");
        }

        [Test]
        public async void GenerateClassDiagram_WheGivenInheritedClassWithBase_ShouldReturnDiagramUrlWithBase()
        {
            var types = new List<Type>
                            {
                                typeof(User), 
                                typeof(Person)
                            };

            // Get image uri
            var imageUri = await new YumlFactory(types).GenerateClassDiagramUri();

            // Verify that the uri is actually an image
            var response = await this.client.GetAsync(imageUri);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Image URI: {0}", imageUri.Replace("http://yuml.me/", "http://yuml.me/edit/"));

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(response.Content.Headers.ContentType.MediaType == "image/png");
        }

        [Test]
        public async void GenerateClassDiagram_WhenGivenInheritedClassWithoutBase_ShouldReturnDiagramUrlWithoutBase()
        {
            var types = new List<Type>
                            {
                                typeof(User)
                            };

            // Get image uri
            var imageUri = await new YumlFactory(types).GenerateClassDiagramUri();

            // Verify that the uri is actually an image
            var response = await this.client.GetAsync(imageUri);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Image URI: {0}", imageUri.Replace("http://yuml.me/", "http://yuml.me/edit/"));

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(response.Content.Headers.ContentType.MediaType == "image/png");
        }

        [Test]
        public async void GenerateClassDiagram_WhenGivenMultipleInheritedClasses_ShouldReturnDiagramUrlWithAllInheritances()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(User), 
                                typeof(Person)
                            };

            // Get image uri
            var imageUri = await new YumlFactory(types).GenerateClassDiagramUri();

            // Verify that the uri is actually an image
            var response = await this.client.GetAsync(imageUri);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Image URI: {0}", imageUri.Replace("http://yuml.me/", "http://yuml.me/edit/"));

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(response.Content.Headers.ContentType.MediaType == "image/png");
        }

        [Test]
        public async void GenerateClassDiagram_WhenGivenClassWithInterface_ShouldReturnDiagramUrl()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(IAdministrator)
                            };

            // Get image uri
            var imageUri = await new YumlFactory(types).GenerateClassDiagramUri();

            // Verify that the uri is actually an image
            var response = await this.client.GetAsync(imageUri);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Image URI: {0}", imageUri.Replace("http://yuml.me/", "http://yuml.me/edit/"));

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(response.Content.Headers.ContentType.MediaType == "image/png");
        }

        [Test]
        public async void GenerateClassDiagram_WhenGivenClassWithClassProperty_ShouldReturnDiagramUrlWithAssociation()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(Domain)
                            };

            // Get image uri
            var imageUri = await new YumlFactory(types).GenerateClassDiagramUri();

            // Verify that the uri is actually an image
            var response = await this.client.GetAsync(imageUri);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Image URI: {0}", imageUri.Replace("http://yuml.me/", "http://yuml.me/edit/"));

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(response.Content.Headers.ContentType.MediaType == "image/png");
        }

        [Test]
        public async void GenerateClassDiagram_WhenGivednClassWithListOfClassProperty_ShouldReturnDiagramUrlWithManyAssociation()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(Role), 
                                typeof(Domain)
                            };

            // Get image uri
            var imageUri = await new YumlFactory(types).GenerateClassDiagramUri();

            // Verify that the uri is actually an image
            var response = await this.client.GetAsync(imageUri);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Image URI: {0}", imageUri.Replace("http://yuml.me/", "http://yuml.me/edit/"));

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(response.Content.Headers.ContentType.MediaType == "image/png");
        }

        [Test]
        public async void GenerateClassDiagram_WhenGivenDetailedClassesWithListOfClassProperty_ShouldReturnDetailedDiagramUrlWithManyAssociation()
        {
            var types = new List<Type>
                            {
                                typeof(Administrator), 
                                typeof(Role), 
                                typeof(Domain)
                            };

            // Get image uri
            var imageUri = await new YumlFactory(types).GenerateClassDiagramUri();

            // Verify that the uri is actually an image
            var response = await this.client.GetAsync(imageUri);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Image URI: {0}", imageUri.Replace("http://yuml.me/", "http://yuml.me/edit/"));

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(response.Content.Headers.ContentType.MediaType == "image/png");
        }

        [Test]
        public async void GenerateClassDiagram_WhenGivenManyDetailedClassesWithListOfClassProperty_ShouldReturnDetailedDiagramUrlWithManyAssociation()
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

            // Get image uri
            var imageUri = await new YumlFactory(types).GenerateClassDiagramUri();

            // Verify that the uri is actually an image
            var response = await this.client.GetAsync(imageUri);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Image URI: {0}", imageUri.Replace("http://yuml.me/", "http://yuml.me/edit/"));

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(response.Content.Headers.ContentType.MediaType == "image/png");
        }

        [Test]
        public async void GenerateClassDiagram_WhenGivenVeryManyDetailedClassesWithListOfClassProperty_ShouldReturnDetailedDiagramUrlWithManyAssociation()
        {
            var types = new List<Type>
                            {
                                typeof(JsonReader),
                                typeof(JsonTextReader),
                                typeof(JsonWriter),
                                typeof(JsonTextWriter)
                            };

            // Get image uri
            var imageUri = await new YumlFactory(types).GenerateClassDiagramUri();

            // Verify that the uri is actually an image
            var response = await this.client.GetAsync(imageUri);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Image URI: {0}", imageUri.Replace("http://yuml.me/", "http://yuml.me/edit/"));

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(response.Content.Headers.ContentType.MediaType == "image/png");
        }
    }
}
