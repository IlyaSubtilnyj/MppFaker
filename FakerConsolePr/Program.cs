using DataTransferObject;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ObjectiveC;
using System.Security.AccessControl;

namespace FakerConsolePr
{
    [Dto]
    public class Foo
    {
        public int x = 5;

        public Foo(Foo foo)
        {
            x = foo.x + 1;
        }
    }

    [Dto]
    class A
    {
        private int x = 0;
        public B b = new();

        public A(B b)
        {
            this.x = b.x + 1;
        }
    }

    [Dto]
    class B {

        public int x = 3;
        private A a;
    }

    class NotDto
    {

    }
   
    internal class Program
    {

        static void Main(string[] args)
        {
            Composer.Load("D:\\workspace\\Visual_Studio_workspace\\studing_workspace\\Faker-proj\\GeneratorsPlugin\\bin\\Debug\\net6.0\\GeneratorsPlugin.dll");

            var inst = Composer.Compose(typeof(List<int>));

            var faker = new Faker();
            var foo = faker.Create<B>();
        }
    }
}