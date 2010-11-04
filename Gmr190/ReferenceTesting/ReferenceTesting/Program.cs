using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceTesting
{
    struct FooStruct
    {
        public int foo;
    }

    class FooClass
    {
        public int foo;

        public FooClass()
        {
            foo = 0;
        }
    }

    class Program
    {
        static void Test(int foo)
        {
            foo = 12;
        }

        static void Test(ref int foo)
        {
            foo = 12;
        }

        static void Test(FooStruct foo)
        {
            foo.foo = 12;
        }

        static void Test(ref FooStruct foo)
        {
            foo.foo = 12;
        }

        static void Test(FooClass foo)
        {
            foo.foo = 12;
        }

        static void Main(string[] args)
        {
            int bar = 0;
            Test(bar);
            Console.WriteLine("atomic type: {0}", bar);

            int foo = 0;
            Test(ref foo);
            Console.WriteLine("ref atomic type: {0}", foo);

            FooStruct fooStruct;
            fooStruct.foo = 0;
            Test(fooStruct);
            Console.WriteLine("struct: {0}", fooStruct.foo);

            FooStruct fooStruct2;
            fooStruct2.foo = 0;
            Test(ref fooStruct2);
            Console.WriteLine("ref struct: {0}", fooStruct2.foo);

            FooClass fooClass = new FooClass();
            fooClass.foo = 0;
            Test(fooClass);
            Console.WriteLine("class: {0}", fooClass.foo);

            Console.ReadKey();
        }
    }
}
