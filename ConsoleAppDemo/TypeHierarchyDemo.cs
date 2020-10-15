namespace ConsoleAppDemo
{
    static class TypeHierarchyDemo
    {
        public static void Run()
        {
            var value = new Derived();
        }
    
        interface IFoo { }
        interface IBar : IFoo { }
        interface IBaz { } 

        class Base : IBar {  }
        class Derived : Base, IBaz {  }
    }
}