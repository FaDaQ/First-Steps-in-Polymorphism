interface ITest1
{

}

interface ITest2 : ITest1
{

}

interface ITest3 : ITest2
{

}

class TestClass1
{

}

class TestClass2 : TestClass1
{

}

static class Extensions
{
    public static IEnumerable<Type> GetInheritedTypes<T>()
    {
        var type = typeof(T);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));

        return types;
    }
}

class Prog
{
    public static void Main()
    {
        foreach (var i in Extensions.GetInheritedTypes<TestClass1>()) //В треугольных скобочках указывается тип, наледников которого вы хотите получить
            Console.WriteLine(i);
    }
}