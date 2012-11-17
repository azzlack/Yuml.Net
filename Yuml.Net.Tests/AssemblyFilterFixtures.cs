namespace Yuml.Net.Test
{
    using NUnit.Framework;

    using Yuml.Net.Test.Objects;

    [TestFixture]
    public class AssemblyFilterFixtures
    {

        [Test]
        public void Can_Determine_Class_Name()
        {
            var reflectionHelper = new AssemblyFilter(typeof(Animal).Assembly);
            Assert.That(reflectionHelper.Types.Contains(typeof(Animal)));
        }

    }
}
