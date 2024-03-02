using System.Numerics;

namespace Cryptography
{
    public class DeffieHellman
    {
        private readonly PrimeGenerator prand = new();
        public BigInteger P { get; private set; } = BigInteger.Zero;
        public BigInteger G { get; private set; } = BigInteger.Zero;
        public BigInteger OwnKey { get; private set; } = BigInteger.Zero;
        public BigInteger PublicKey { get; private set; } = BigInteger.Zero;
        public BigInteger PrivateKey { get; private set; } = BigInteger.Zero;
        private bool isGenerated = false;

        public DeffieHellman() { }
        public DeffieHellman(BigInteger p, BigInteger g)
        {
            this.P = p;
            this.G = g;
            isGenerated = true;
            return;
        }

        public void GenerateParameters()
        {
            (P, G) = GeneratePG();
            isGenerated = true;
            return;
        }

        // Генерация открытого ключа X и собственного числа x
        public bool GeneratePublicKey()
        {
            if (!isGenerated) return false;
            OwnKey = prand.RandNum(P);
            PublicKey = MyMath.ModExp(G, OwnKey, P);

            return true;
        }

        // Расчёт общего ключа исходя из открытого ключа другого
        public bool CalculatePrivateKey(DeffieHellman other) => CalculatePrivateKey(other.PublicKey);
        public bool CalculatePrivateKey(BigInteger publicKey)
        {
            if (OwnKey == BigInteger.Zero) return false;
            PrivateKey = MyMath.ModExp(publicKey, OwnKey, P);
            return true;
        }

        private (BigInteger, BigInteger) GeneratePG()
        {
            BigInteger p, g, n, q = prand.GeneratePrime();

            do
            {
                n = prand.RandNum(new BigInteger(10000000));
                p = n * q + 1;
            } while (!PrimeGenerator.IsPrime(p));

            do
            {
                var a = prand.RandNum(p);
                g = MyMath.ModExp(a, n, p);
            } while (g == 1);

            return (p, g);
        }
    }
}
