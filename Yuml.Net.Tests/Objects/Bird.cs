namespace Yuml.Net.Test.Objects
{
    using System.Collections.Generic;

    public class Bird : Animal
    {
        public string WingSpan { get; set; }
    }
    public class Eagle : Bird, IBirdOfPray, IAnimalPray
    {
        public Claw Claw { get; set; }
        public IList<Wing> Wings { get; set; }
    }
    public class Claw
    {
    }

    public class Wing
    {
    }

    public interface IBirdOfPray
    {  
    }

    public interface IAnimalPray
    {
    }

}
