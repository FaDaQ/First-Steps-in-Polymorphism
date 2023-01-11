using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;

static class MyMethods
{
    public static string DivideDigits(this int num)
    {
        return num.ToString("#,#", new CultureInfo("ru-RU"));
    }
    public static string DivideDigits(this double num)
    {
        return num.ToString("#,#", new CultureInfo("ru-RU"));
    }

    public static IEnumerable<Type> GetInheritedTypes<T>()
    {
        var type = typeof(T);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));

        return types;
    }

    public static void PrintInheritedTypes<T>(out int count)
    {
        int typesCount = 1;
        foreach (var i in GetInheritedTypes<T>())
        {
            if (typesCount != 1)
            {
                Console.WriteLine($"{typesCount - 1}. {i}");
                typesCount++;
            }
            else
                typesCount++;
        }
        count = typesCount - 2;
    }
}

#region Engines
interface IRocketEngine
{
    public void PrintSpecs(bool printMessage = true)
    {
        Console.WriteLine("\n\n\nВызван метод интерфейса!\nВозможно неполностью указаны параметры метода!\n\n");
    }
    public virtual void Start()
    {
        Console.WriteLine($"\n\n\nВызван метод интерфейса!\nВозможно неполностью указаны параметры метода!\nИли метод нереализован\n\n");
    }

    public void Off()
    {
        
    }

    public bool EngineOn { get; }
    public string Manufacturer { get; }
    public double MaxPower { get; }
}
class DeffRocketEngine : IRocketEngine
{
    public DeffRocketEngine()
    {
        Manufacturer = "NA";
        MaxPower = 0;
    }
    public DeffRocketEngine(string manufacturer, double maxPower)
    {
        Manufacturer = manufacturer;
        MaxPower = maxPower;
    }

    public void Start()
    {
        Console.WriteLine($"Обычный двигатель ракеты заведён!");
        EngineOn = true;
    }

    public void Off()
    {
        Console.WriteLine($"Обычный двигатель ракеты отключён!");
        EngineOn = false;
    }

    public void PrintSpecs(bool printMessage = true)
    {
        Console.WriteLine(printMessage ? $"\n--------|Спецификации обычного двигателя|--------\n\n" : "" +
                          $"\tСостояние двигателя: " + (EngineOn ? "On\n" : "Off\n") +
                          $"\tПроизводитель двигателя: {Manufacturer}\n" +
                          $"\tMax. мощность двигателя: {MaxPower.DivideDigits()} ватт/с\n" +
                          (printMessage ? "\n--------|Спецификации обычного двигателя|--------\n\n)" : ""));
    }

    public bool EngineOn { get; private set; }
    public string Manufacturer { get; private set; }
    public double MaxPower { get; }
}

class DiminaJopaEngine : IRocketEngine
{
    public DiminaJopaEngine()
    {
        Manufacturer = "Дима Space-Enjoeer";
        MaxPower = 30000000;
    }

    public void Start()
    {
        Console.WriteLine($"Димина жопа сгорела от программирования! На её тяге можно покорить галактику Альфа Центавра!\n");
        EngineOn = true;
    }

    public void Off()
    {
        Console.WriteLine();
        EngineOn = false;
    }

    public void PrintSpecs(bool printMessage = true)
    {
        Console.WriteLine(printMessage ? $"\n--------|Спецификации двигателя Димина-Жопа|--------\n\n" : "" +
                          $"\tСостояние двигателя: " + (EngineOn ? "On\n" : "Off\n") +
                          $"\tПроизводитель двигателя: {Manufacturer}\n" +
                          $"\tMax. мощность двигателя: {MaxPower}\n ватт/с" +
                          (printMessage ? "\n--------|Спецификации двигателя Димина-Жопа|--------\n\n)" : ""));
    }

    public bool EngineOn { get; private set; }
    public string Manufacturer { get; private set; }
    public double MaxPower { get; }
}
#endregion
#region Rockets
abstract class ARocket
{
    public ARocket()
    {
        Name = "NA";
        Engine = new DeffRocketEngine();
        Mass = 0;
        MaxSpeed = 0;
    }
    public ARocket(string name, IRocketEngine engine, double mass) 
    {
        Name = name;
        Engine = engine;
        Mass = mass;
        MaxSpeed = Engine.MaxPower / Mass;
    }

    public virtual void PrintSpecs()
    {
        Console.WriteLine($"\n\tИмя: {Name}\n" +
                          $"\tМасса: {Mass} тонн\n" +
                          $"\tMax. скорость ракеты: {Math.Ceiling(MaxSpeed).DivideDigits()} км/ч");
        Engine.PrintSpecs(false);

    }

    /*Метод для вывода сообщения в стиле "----------|Спецификации обычной ракеты|----------|"
      Просто чтобы не повторять одних и тех же слов*/
    protected void PrintRocketNameSpecs(string name)
    {
        Console.WriteLine($"----------|Спецификации {name} ракеты|----------|");
    }

    public string Name { get; }
    public IRocketEngine Engine { get; }
    protected double MaxSpeed { get; set; }
    public double Mass { get; }

}

class Rocket : ARocket
{
    public Rocket() : base() { }
    public Rocket(string name, IRocketEngine engine, double mass) : base(name, engine, mass)
    {
        MaxSpeed *= 150;
    }

    override public void PrintSpecs() 
    {
        PrintRocketNameSpecs("обычной");
        base.PrintSpecs();
        PrintRocketNameSpecs("обычной");

    }
}

class SuperRocket : ARocket
{
    public SuperRocket() : base() { }
    public SuperRocket(string name, IRocketEngine engine, double mass) : base(name, engine, mass) 
    {
        
    }

    override public void PrintSpecs()
    {
        PrintRocketNameSpecs("SUPER");
        base.PrintSpecs();
        PrintRocketNameSpecs("SUPER");

    }

}

class SpaceX : ARocket
{
    public SpaceX() : base() { }
    public SpaceX(string name, IRocketEngine engine, double mass) : base(name, engine, mass)
    {
        
    }

    override public void PrintSpecs()
    {
        PrintRocketNameSpecs("SpaceX");
        base.PrintSpecs();
        PrintRocketNameSpecs("SpaceX");

    }
}
#endregion

class Prog
{
    public static ARocket CreateNewRocket()
    {
        IRocketEngine rocketEngine = new DeffRocketEngine();
        int engineCount;
        Console.WriteLine("-----|Выберите тип двигателя|-----");
        MyMethods.PrintInheritedTypes<IRocketEngine>(out engineCount);
        Console.WriteLine("-----|Выберите тип двигателя|-----");

        int engineType;
    inputEngine:
        Console.Write("Ввод: ");
        if (!int.TryParse(Console.ReadLine(), out engineType) || engineType > engineCount)
        {
            Console.WriteLine("\nНекорректный ввод!\n");
            goto inputEngine;
        }


        Console.WriteLine("\n-----|Выберите производителя двигателя|-----");
        int manufCount = 1;
        foreach (var item in manufacturerList)
        {
            Console.WriteLine($"{manufCount++}. {item}");
        }
        Console.WriteLine("-----|Выберите производителя двигателя|-----\n");

        int engineManuf;
        double enginePower = 0;
    inputEngineManuf:
        Console.Write("Ввод: ");
        if (!int.TryParse(Console.ReadLine(), out engineManuf) || engineManuf > manufacturerList.Count)
        {
            Console.WriteLine("\nНекорректный ввод!\n");
            goto inputEngineManuf;
        }
        else if (engineTypesList[engineType - 1] != new DiminaJopaEngine().GetType())
        {
        inputEnginePower:
            Console.Write("Введите мощность двигателя: ");
            if (!double.TryParse(Console.ReadLine(), out enginePower))
            {
                Console.WriteLine("\nНекорректный ввод!\n");
                goto inputEnginePower;
            }
        }


        switch (engineType)
        {
            case 1:
                rocketEngine = new DeffRocketEngine(manufacturerList[engineManuf - 1], enginePower);
                break;
            case 2:
                rocketEngine = new DiminaJopaEngine();
                break;

        }

        int rocketCount;
        Console.WriteLine("\n-----|Выберите тип ракеты|-----");
        MyMethods.PrintInheritedTypes<ARocket>(out rocketCount);
        Console.WriteLine("-----|Выберите тип ракеты|-----");

        int rocketType;
    inputRocket:
        Console.Write("Ввод: ");
        if (!int.TryParse(Console.ReadLine(), out rocketType) || rocketType > rocketCount)
        {
            Console.WriteLine("\nНекорректный ввод!\n");
            goto inputRocket;
        }
        Console.Write("Введите имя ракеты: ");
        string? rocketName = Console.ReadLine();

        Console.Write("Введите массу ракеты: ");
        double rocketMass;
        if (!double.TryParse(Console.ReadLine(), out rocketMass))
        {
            Console.WriteLine("\nНекорректный ввод!\n");
            goto inputEngine;
        }

        switch (rocketType)
        {
            case 1:
                return new Rocket(rocketName, rocketEngine, rocketMass);
            case 2:
                return new SuperRocket(rocketName, rocketEngine, rocketMass);
            case 3:
                return new SpaceX(rocketName, rocketEngine, rocketMass);
            default:
                return new Rocket();
        }

        
    }
    
    public static void Main()
    {
        engineTypesList.RemoveAt(0);
        rocketTypesList.RemoveAt(0);

        rocketList.Add(CreateNewRocket());
        Console.WriteLine();
        rocketList[0].Engine.Start();
        Console.WriteLine();
        rocketList[0].PrintSpecs();


    }
    public static List<string> manufacturerList = new List<string>() { "Хуйндай", "Самсука", "Нихия", "SpaceX" };
    public static List<ARocket> rocketList = new List<ARocket>();
    public static List<Type> engineTypesList = new List<Type>(MyMethods.GetInheritedTypes<IRocketEngine>());
    public static List<Type> rocketTypesList = new List<Type>(MyMethods.GetInheritedTypes<ARocket>()); 


}