using System.Numerics;

namespace Cryptography
{
    public class DeffieHellman
    {
        private readonly PrimeGenerator prand = new();
        public BigInteger P { get; private set; } = BigInteger.Zero;
        public BigInteger G { get; private set; } = BigInteger.Zero;
        public BigInteger PublicKey { get; private set; } = BigInteger.Zero;
        public BigInteger PrivateKey { get; private set; } = BigInteger.Zero;
        public BigInteger GeneralKey { get; private set; } = BigInteger.Zero;
        private bool isGenerated = false;

        public DeffieHellman() { }
        public DeffieHellman(BigInteger p, BigInteger g)
        {
            this.P = p;
            this.G = g;
            isGenerated = true;
            return;
        }

        // Генерация начальных параметров
        public void GenerateParameters()
        {
            (P, G) = GeneratePG();
            isGenerated = true;
            return;
        }

        // Генерация открытого ключа X и собственного числа x
        public bool GenerateInstanceKeys()
        {
            if (!isGenerated) return false;
            PrivateKey = prand.RandNum(P);
            PublicKey = MyMath.ModExp(G, PrivateKey, P);

            return true;
        }

        // Расчёт общего ключа исходя из открытого ключа другого пользователя
        public bool CalculateGeneralKey(DeffieHellman other) => CalculateGeneralKey(other.PublicKey);
        public bool CalculateGeneralKey(BigInteger publicKey)
        {
            if (PrivateKey == BigInteger.Zero) return false;
            GeneralKey = MyMath.ModExp(publicKey, PrivateKey, P);
            return true;
        }

        // Генерация параметров P и G
        private (BigInteger, BigInteger) GeneratePG()
        {
            BigInteger p = 2, g, n, q = prand.GeneratePrime();

            // Получаем простое p на основе случайно сгенерированных q и n
            do
            {
                n = prand.RandNum(new BigInteger(10000000));
                p = n * q + 1;
            } while (!PrimeGenerator.IsPrime(p));

            // Генерация g на основе случайного a из мультипликативной группы вычетов
            do
            {
                BigInteger a;
                do a = prand.RandNum(p);
                while (MyMath.Euclid(p, a) != 1);
                g = MyMath.ModExp(a, n, p);
            } while (g == 1);

            return (p, g);
        }
    }
}
