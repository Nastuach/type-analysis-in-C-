using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;



namespace Lab1
{
    //public record Person{string FirstName, string LastName};
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Информация по типам:");
                Console.WriteLine("1 – Общая информация по типам");
                Console.WriteLine("2 – Выбрать тип из списка");
                Console.WriteLine("3 – Параметры консоли");
                Console.WriteLine("0 - Выход из программы");

                var key = Console.ReadKey(true).KeyChar;

                switch (key)
                {
                    case '1': ShowAllTypeInfo(); break;
                    case '2': SelectType(); break;
                    case '3': ChangeConsoleView(); break;
                    case '0':return;
                    default: break;
                }
            }
        }

        static void ShowAllTypeInfo()
        {
            var types = GetAllTypes();
            int nRefTypes = types.Count(t => t.IsClass);
            int nValueTypes = types.Count(t => t.IsValueType);

            Console.Clear();
            Console.WriteLine("Общая информация по типам");
            Console.WriteLine($"Подключенные сборки: {AppDomain.CurrentDomain.GetAssemblies().Length}");
            Console.WriteLine($"Всего типов по всем подключенным сборкам: {types.Count}");
            Console.WriteLine($"Ссылочные типы (только классы): {nRefTypes}");
            Console.WriteLine($"Значимые типы: {nValueTypes}");

            var longestProperty = TypeUtils.GetTheLongestPubProperty(types);
            Console.WriteLine($"Самое длинное название свойства: {longestProperty?.Name}");

            var typeWithMaxConstructors = TypeUtils.GetTypeWithMaxConstructors(types);
            Console.WriteLine($"Тип с наибольшим числом конструкторов: {typeWithMaxConstructors?.Name}");

            var methodWithArgVariety = TypeUtils.GetMethodWithArgVariety(types);
            Console.WriteLine($"Метод с наибольшим разнообразием аргументов: {methodWithArgVariety?.Name}");

            Console.WriteLine("\nНажмите любую клавишу для возврата в главное меню...");
            Console.ReadKey();
        }

        static void SelectType()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Информация по типам");
                Console.WriteLine("Выберите тип:");
                Console.WriteLine("1 – uint");
                Console.WriteLine("2 – int");
                Console.WriteLine("3 – long");
                Console.WriteLine("4 – float");
                Console.WriteLine("5 – double");
                Console.WriteLine("6 – char");
                Console.WriteLine("7 – string");
                Console.WriteLine("8 – record");
                Console.WriteLine("9 – Tuple<int, string>");
                Console.WriteLine("0 – Выход в главное меню");

                var key = Console.ReadKey(true).KeyChar;

                Type selectedType = null;
                switch (key)
                {
                    case '1':
                        selectedType = typeof(uint);
                        break;
                    case '2':
                        selectedType = typeof(int);
                        break;
                    case '3':
                        selectedType = typeof(long);
                        break;
                    case '4':
                        selectedType = typeof(float);
                        break;
                    case '5':
                        selectedType = typeof(double);
                        break;
                    case '6':
                        selectedType = typeof(char);
                        break;
                    case '7':
                        selectedType = typeof(string);
                        break;
                    case '8':
                        //selectedType = typeof(Person);
                        break;
                    case '9':
                        selectedType = typeof(Tuple<int, string>);
                        break;
                    case '0':
                        return;
                    default:
                        continue;
                }

                if (selectedType != null)
                {
                    ShowTypeInfo(selectedType);
                }
            }
        }

        static void ShowTypeInfo(Type type)
        {
            Console.Clear();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            Console.WriteLine($"Информация по типу: {type.FullName}");
            Console.WriteLine($"Значимый тип: {(type.IsValueType ? "+" : "-")}");
            Console.WriteLine($"Пространство имен: {type.Namespace}");
            Console.WriteLine($"Сборка: {type.Assembly.GetName().Name}");
            Console.WriteLine($"Общее число элементов: {type.GetMembers(flags).Length}");
            Console.WriteLine($"Число методов: {type.GetMethods(flags).Length}");
            Console.WriteLine($"Число свойств: {type.GetProperties(flags).Length}");
            Console.WriteLine($"Число полей: {type.GetFields(flags).Length}");
            Console.WriteLine($"Список полей: {string.Join(", ", type.GetFields(flags).Select(f => f.Name))}");
            Console.WriteLine($"Список свойств: {string.Join(", ", type.GetProperties(flags).Select(p => p.Name))}");

            Console.WriteLine("\nНажмите ‘M’ для вывода дополнительной информации по методам:");
            Console.WriteLine("Нажмите ‘0’ для выхода в главное меню");

            var key = Console.ReadKey(true).KeyChar;
            if (key == 'M' || key == 'm')
            {
                ShowMethodInfo(type);
            }
        }

        static void ShowMethodInfo(Type type)
        {
            Assembly myAsm = Assembly.GetExecutingAssembly();
            Type[] thisAssemblyTypes = myAsm.GetTypes();
            List<MethodInfo> allMethods = new List<MethodInfo>();
            foreach (var t in thisAssemblyTypes)
            {
                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
                allMethods.AddRange(t.GetMethods(flags));
            }
            var methodGroups = allMethods.GroupBy(m => m.ReturnType);
            Console.Clear();
            Console.WriteLine("Методы, сгруппированные по типам возврата:");

            foreach (var group in methodGroups)
            {
                Console.WriteLine($"\nТип возврата: {group.Key.Name}");
                Console.WriteLine($"Число методов: {group.Count()}");

                var uniqueMethodNames = group.Select(m => m.Name).Distinct();

                Console.WriteLine($"Уникальные методы: {string.Join(", ", uniqueMethodNames)}");
            }

            Console.WriteLine("\nНажмите любую клавишу для возврата...");
            Console.ReadKey();
        }

        static void ChangeConsoleView()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Параметры консоли:");
                Console.WriteLine("1 – Изменить цвет шрифта");
                Console.WriteLine("2 – Изменить цвет фона");
                Console.WriteLine("0 – Выход в главное меню");

                var key = Console.ReadKey(true).KeyChar;

                switch (key)
                {
                    case '1':
                        ChangeFontColor();
                        break;
                    case '2':
                        ChangeBackgroundColor();
                        break;
                    case '0':
                        return;
                    default:
                        break;
                }
            }
        }
        static void ChangeFontColor()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите цвет шрифта:");
                Console.WriteLine("1 – Черный");
                Console.WriteLine("2 – Белый");
                Console.WriteLine("3 – Красный");
                Console.WriteLine("4 – Зеленый");
                Console.WriteLine("5 – Синий");
                Console.WriteLine("0 – Отмена");

                var key = Console.ReadKey(true).KeyChar;

                ConsoleColor selectedColor;
                switch (key)
                {
                    case '1':
                        selectedColor = ConsoleColor.Black;
                        break;
                    case '2':
                        selectedColor = ConsoleColor.White;
                        break;
                    case '3':
                        selectedColor = ConsoleColor.Red;
                        break;
                    case '4':
                        selectedColor = ConsoleColor.Green;
                        break;
                    case '5':
                        selectedColor = ConsoleColor.Blue;
                        break;
                    case '0':
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        Console.ReadKey();
                        continue;
                }

                if (selectedColor == Console.BackgroundColor)
                {
                    Console.WriteLine("Ошибка: Цвет шрифта не может совпадать с цветом фона. Выберите другой цвет.");
                    Console.ReadKey();
                }
                else
                {
                    Console.ForegroundColor = selectedColor;
                    Console.Clear();
                    break;
                }
            }
        }

        static void ChangeBackgroundColor()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите цвет фона:");
                Console.WriteLine("1 – Черный");
                Console.WriteLine("2 – Белый");
                Console.WriteLine("3 – Красный");
                Console.WriteLine("4 – Зеленый");
                Console.WriteLine("5 – Синий");
                Console.WriteLine("0 – Отмена");

                var key = Console.ReadKey(true).KeyChar;

                ConsoleColor selectedColor;
                switch (key)
                {
                    case '1':
                        selectedColor = ConsoleColor.Black;
                        break;
                    case '2':
                        selectedColor = ConsoleColor.White;
                        break;
                    case '3':
                        selectedColor = ConsoleColor.Red;
                        break;
                    case '4':
                        selectedColor = ConsoleColor.Green;
                        break;
                    case '5':
                        selectedColor = ConsoleColor.Blue;
                        break;
                    case '0':
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        Console.ReadKey();
                        continue;
                }

                if (selectedColor == Console.ForegroundColor)
                {
                    Console.WriteLine("Ошибка: Цвет фона не может совпадать с цветом шрифта. Выберите другой цвет.");
                    Console.ReadKey();
                }
                else
                {
                    Console.BackgroundColor = selectedColor;
                    Console.Clear();
                    break;
                }
            }
        }

        static List<Type> GetAllTypes()
        {
            List<Type> types = new List<Type>();
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                types.AddRange(asm.GetTypes());
            }
            return types;
        }

    }

    public static class TypeUtils
    {
        public static PropertyInfo GetTheLongestPubProperty(List<Type> types)
        {
            return types.SelectMany(t => t.GetProperties()).OrderByDescending(p => p.Name.Length).FirstOrDefault();
            throw new NotImplementedException();
        }
        public static Type GetTypeWithMaxConstructors(List<Type> types)
        {
            return types.OrderByDescending(t => t.GetConstructors().Length).FirstOrDefault();
            throw new NotImplementedException();
        }
        public static MethodInfo GetMethodWithArgVariety(List<Type> types)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            return types.SelectMany(t => t.GetMethods(flags)).OrderByDescending(m => m.GetParameters().Select(p => p.ParameterType).Distinct().Count()).FirstOrDefault();
            throw new NotImplementedException();
        }
    }
}