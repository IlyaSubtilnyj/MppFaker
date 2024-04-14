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

        public int y;
        public int x = 0;
        public int z { get; set; }

        private Foo(int x) { y = 25; this.x = x; }

        public Foo(Foo foo) { 
            x = foo.x + 1;
        }

        public Foo(IEnumerable<int> x, int y) { 
            this.y = y;
        }

    }

    class Test
    {
        public int y;
    }

    [Dto]
    class A
    {
        private int x = 0;

        public B b { 
            set {
                x += value.x;
            } 
        }
    }

    [Dto]
    class B {

        public int x = 3;
        public A a { get; set; }
    }

    internal class Program
    {

        static void Main(string[] args)
        {


            //var lol2 = Composer.Formulate(typeof(List<List<int>>));
            var foo = DataTransferObject.Faker.Create<A>();

        }
    }
}