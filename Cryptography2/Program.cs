namespace Cryptography
{
    class Program
    {
        public static void Main()
        {
            DeffieHellman unit1 = new();
            DeffieHellman unit2 = new();
            string answer;

            while (true)
            {
                Console.WriteLine("\n-----------------\n");
                Console.WriteLine("Выберите опцию:");
                Console.WriteLine("1.Сгенерировать p, g");
                Console.WriteLine("2.Сгенерировать открытые ключи для двух сторон");
                Console.WriteLine("3.Получить общий ключ(обмен открытыми ключами)");
                Console.WriteLine("4. Выход");

                do answer = Console.ReadLine();
                while (answer != "1" && answer != "2" &&
                    answer != "3" && answer != "4");

                switch (answer)
                {
                    case "1":
                        unit1.GenerateParameters();
                        unit2 = new DeffieHellman(unit1.P, unit1.G);
                        Console.WriteLine($"\np: {unit1.P}; \ng: {unit1.G};");
                        break;
                    case "2":
                        if (unit1.GenerateInstanceKeys() && unit2.GenerateInstanceKeys())
                            Console.WriteLine($"Ключи были сгенерированы: " +
                                $"\nОткрытые: X: {unit1.PublicKey} \tY: {unit2.PublicKey}" +
                                $"\nЗакрытые: x: {unit1.PrivateKey} \ty: {unit2.PrivateKey}");
                        else
                            Console.WriteLine("\nСначала сгенерируйте p и g.");
                        break;
                    case "3":
                        if (unit1.CalculateGeneralKey(unit2) && unit2.CalculateGeneralKey(unit1)) 
                        {
                            Console.WriteLine($"Общий ключ: (должен быть одинаковым)" +
                                $"\nПервый: \t{unit1.GeneralKey}" +
                                $"\nВторой: \t{unit2.GeneralKey}");
                        }
                        else
                            Console.WriteLine("\nСначала сгенерируйте открытые ключи.");
                        break;
                    case "4":
                        return;
                }
            }
        }
    }
}