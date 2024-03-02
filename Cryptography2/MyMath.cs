using System.Numerics;

namespace Cryptography
{
    public static class MyMath
    {
        // Возведение в степень по модулю
        public static BigInteger ModExp(BigInteger a, BigInteger b, BigInteger n)
        {
            var c = a;
            BigInteger d = 1;
            while (b > 0)
            {

                if (b % 2 == 1)
                    d = (d * c) % n;
                b = b / 2;
                c = (c * c) % n;
            }

            return d;
        }

        // Функция Эйлера
        // Количество меньших взаимно простых чисел
        public static BigInteger EulerFunc(BigInteger p, BigInteger q) => (p - 1) * (q - 1);

        // Алгоритм Евклида
        // Нахождение НОД
        public static BigInteger Euclid(BigInteger a, BigInteger b)
        {
            BigInteger t;
            while (b > 0)
            {
                t = b;
                b = a % b;
                a = t;
            }

            return a;
        }

        // Расширенный алгоритм Евклида
        // Нахождение НОД и линейной комбинации
        public static (BigInteger, BigInteger, BigInteger) EuclidExtended(BigInteger a, BigInteger b)
        {
            if (b == 0)
                return (a, 1, 0);

            BigInteger d, x, y, x1, y1;
            (d, x1, y1) = EuclidExtended(b, a % b);
            x = y1;
            y = x1 - a / b * y1;

            return (d, x, y);
        }

        // Получить взаимно простое число относительно a
        public static BigInteger GetRelativelyPrime(BigInteger a)
        {
            var rand = new PrimeGenerator();
            BigInteger b;

            while (true)
            {
                b = rand.GeneratePrime();
                if (b >= a && b <= 2) continue;

                for (b = b % a; b < a; ++b)
                {
                    if (Euclid(a, b) == 1)
                        return b;
                }
            }
        }

    }
}
