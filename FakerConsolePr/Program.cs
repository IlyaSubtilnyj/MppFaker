using DataTransferObject;

namespace FakerConsolePr
{
    public class Foo : IDto
    {

        public int y;
        public string x;

        //private Foo() { y = 25; }

        public Foo(IEnumerable<int> x, int y) { 
            this.y = y;
        }

    }
    internal class Program
    {

        class Test
        {
            public int y;
        }

        static void Main(string[] args)
        {
            var foo = DataTransferObject.Faker.Create<Foo>();  
        }
    }
}