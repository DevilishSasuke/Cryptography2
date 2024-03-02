using System.Numerics;
using System.Security.AccessControl;

namespace Cryptography
{
    public class CryptoRSA
    {
        public BigInteger p { get; private set; } = BigInteger.Zero;
        public BigInteger q { get; private set; } = BigInteger.Zero;
        public BigInteger N { get; private set; } = BigInteger.Zero;
        public BigInteger e { get; private set; } = BigInteger.Zero;
        public BigInteger d { get; private set; } = BigInteger.Zero;
        private readonly PrimeGenerator rand = new PrimeGenerator();

        public void GenerateNewKeys()
        {
            p = rand.GeneratePrime();
            q = rand.GeneratePrime();

            N = p * q;

            BigInteger phi = EulerFunc(p, q);
            e = GetE(phi);
            d = phi - BigInteger.Abs(EuclidExtended(phi, e));
            Console.WriteLine(d);
            d = (1 / e) % phi;
            Console.WriteLine(d);
            Console.WriteLine(EuclidExtended(336, 90));
        }

        public BigInteger EncryptText(BigInteger text) => N > 0 ? 
            ModExp(text, e, N): 
            throw new Exception("Keys must be generated");

        public BigInteger DecryptText(BigInteger text) => N > 0 ? 
            ModExp(text, d, N): 
            throw new Exception("Keys must be generated");

        public (BigInteger, BigInteger) GetPublicKey() => (e, N);
        public (BigInteger, BigInteger) GetPrivateKey() => (d, N);

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

        public BigInteger EulerFunc(BigInteger p, BigInteger q)
        {
            return (p - 1) * (q - 1);
        }

        public static BigInteger EuclidExtended(BigInteger a, BigInteger b)
        {
            if (b == 0)
                return ()

            BigInteger d, x, y, x1, y1;

            return;

            BigInteger q, r, x, y, x1 = 0, y2 = 0, x2 = 1, y1 = 1;
            while (b > 0)
            {
                q = a / b;
                r = a - q * b;
                x = x2 - q * x1;
                y = y2 - q * y1;
                a = b;
                b = r;
                x2 = x1;
                x1 = x;
                y2 = y1;
                y1 = y;
            }

            if (x2 < y2) return x2;
            return y2;
        }

        public BigInteger GetRelativelyPrime(BigInteger a)
        {
            BigInteger b;

            while (true)
            {
                b = rand.GeneratePrime();
                for (b = b % a; b < a; ++b)
                {
                    if (GCD(a, b) == 1)
                        return b;
                }
            }
        }

        private static BigInteger GCD(BigInteger num1, BigInteger num2)
        {
            BigInteger remainder;

            while (num2 != 0)
            {
                remainder = num1 % num2;
                num1 = num2;
                num2 = remainder;
            }

            return num1;
        }

        public BigInteger GetE(BigInteger phi)
        {
            var answer = Program.ChooseE();
            if (answer == "1")
                return Program.GetNumberFromConsole();
            while (true) 
            {
                var candidate = rand.randNum(24);
                if (candidate % 2 == 0) continue;

                if (GCD(candidate, phi) == 1)
                    return candidate;
            }
        }
    }
}
