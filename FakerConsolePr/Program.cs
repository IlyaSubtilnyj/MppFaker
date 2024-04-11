using DataTransferObject;

namespace FakerConsolePr
{
    public class Foo : IDto
    {

        public int y;

        public Foo() { }
        public Foo(int x) { }

        public Foo(string x) { }

    }
    internal class Program
    {

        class Test
        {
            public int y;
        }

        static void Main(string[] args)
        {

            //Faker.Create<IDto>();
            Foo foo = Faker.Create<Foo>();
        }
    }
}