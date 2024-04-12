using DataTransferObject;

namespace FakerConsolePr
{
    public class Foo : IDto
    {

        public int y;

        public Foo(int y) { 
            this.y = y;
        }

        public Foo(string x) { }

        public Foo() { }

    }
    internal class Program
    {

        class Test
        {
            public int y;
        }

        static void Main(string[] args)
        {
            Foo foo = DataTransferObject.Faker.Create<Foo>();  
        }
    }
}