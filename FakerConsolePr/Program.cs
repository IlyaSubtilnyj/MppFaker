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

        //public Foo f;

        //public Foo foo1 { get; set; }

        //private Foo(int x) { y = 25; this.x = x; }

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
        public Test(int y) { this.y = y; }
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

        //public B B {
        //    get { return b; }
        //    set {
        //        B.x += x;
        //    } 
        //}
    }

    [Dto]
    class B {

        public int x = 3;

        public A a;

        //public A a { get; set; }
       //public C c { get; set; }
    }

    [Dto]
    class C
    {

        public A a { get; set; }
    }

   
    internal class Program
    {

        static void Main(string[] args)
        {
           Composer.Load("D:\\workspace\\Visual_Studio_workspace\\studing_workspace\\Faker-proj\\GeneratorsPlugin\\bin\\Debug\\net6.0\\GeneratorsPlugin.dll");

            var faker = new Faker();
            A foo = faker.Create<A>();
        }
    }
}