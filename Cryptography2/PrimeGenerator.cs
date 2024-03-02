using System.Numerics;

namespace Cryptography
{
    // Генератор псевдопростых чисел
    public class PrimeGenerator
    {
        private static readonly Random random = new();
        private BigInteger Candidate { get; set; }

        // Получить новое простое число
        public BigInteger GeneratePrime()
        {
            while (true)
            {
                Candidate = RandNum();
                if (Candidate == 0) continue;

                if (IsPrime(Candidate))
                {
                    // Проверка простого числа на "надёжность"/"безопасность"
                    var safePrime = Candidate * 2 + 1;
                    if (IsPrime(safePrime))
                       return safePrime;
                }    
            }

        }

        // Генерация случайного числа
        // len - длина битов в числе
        public static BigInteger RandNum(int len = 0)
        {
            len = len > 7 ? len / 8 : random.Next(100, 130) / 8;
            var bytes = new byte[len];

            random.NextBytes(bytes);
            bytes[^1] &= (byte)0x7F; // Смена знака на положительный

            return new BigInteger(bytes);
        }

        // Генерация случайного BigInteger
        // в диапазоне [1, N-1] включительно
        public BigInteger RandNum(BigInteger N)
        {
            BigInteger candidate;
            var bytes = N.ToByteArray();

            do
            {
                random.NextBytes(bytes);
                candidate = new BigInteger(bytes);
            } while (candidate >= N && candidate <= 0);

            return candidate;
        }

        // Проверка на простоту поиском чисел-делителей
        // из решета Эратосфена
        public static bool IsPrime(BigInteger number)
        {
            var sieve = EratosthenesSieve.GetSieve();

            // Проходим по решету простых чисел
            foreach (var prime in sieve)
            {
                if (prime >= number)
                    return true;
                if (number % prime == 0)
                    return false;
            }

            return RabinMiller(number);
        }

        public static bool RabinMiller(BigInteger number) => RabinMiller(number, 50);

        // Проверка Рабина Миллера
        public static bool RabinMiller(BigInteger number, int steps)
        {
            var b = number - 1;
            var k = -1;
            BigInteger d, x;
            BigInteger[] beta = new BigInteger[150];
            do
            {
                k++;
                beta[k] = b % 2;
                b /= 2;
            } while (b > 0);

            // Делаем steps попыток обнаружить делители
            for (int j = 0; j < steps; j++)
            {
                var a = RandNum();
                // Взаимно простые?
                if (MyMath.Euclid(a, number) > 1)
                    return false;
                d = 1;
                for (int i = k; i >= 0; i--)
                {
                    x = d;
                    d = (d * d) % number;
                    if (d == 1 && x != 1 && x != number - 1)
                        return false;
                    if (beta[i] == 1)
                        d = (d * a) % number;
                }
                if (d != 1)
                    return false;
            }

            return true;
        }
    }
}