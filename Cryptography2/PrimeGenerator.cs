using System.Numerics;

namespace Cryptography
{
    // Генератор псевдопростых чисел
    public class PrimeGenerator
    {
        private readonly Random random = new();
        private BigInteger Candidate { get; set; }

        // Получить новое простое число
        public BigInteger GeneratePrime()
        {
            int k = 50;

            while (true)
            {
                Candidate = RandNum();
                if (Candidate == 0) continue;

                if (IsLikelyPrime(Candidate) && RabinMiller(Candidate, k))
                    return Candidate;
            }

        }

        // Генерация случайного числа
        // len - длина битов в числе
        public BigInteger RandNum(int len = 0)
        {
            len = len > 7 ? len / 8 : random.Next(100, 130) / 8;
            var bytes = new byte[len];

            random.NextBytes(bytes);
            bytes[^1] &= (byte)0x7F; // Смена знака на положительный

            return new BigInteger(bytes);
        }

        // Проверка на простоту поиском чисел-делителей
        // из решета Эратосфена
        public static bool IsLikelyPrime(BigInteger number)
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

            return true;
        }

        // Проверка Рабина Миллера
        public bool RabinMiller(BigInteger number, int steps)
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