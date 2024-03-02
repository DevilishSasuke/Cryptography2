using System.Numerics;

namespace Cryptography
{
    public class PrimeGenerator
    {
        private readonly Random random = new Random();
        private BigInteger candidate { get; set; }
        public BigInteger GeneratePrime()
        {
            int k = 50;

            while (true)
            {
                candidate = randNum();
                if (candidate == 0) continue;

                if (IsLikelyPrime(candidate))
                    if (RabinMiller(candidate, k))
                        return candidate;
            }

        }

        public BigInteger randNum(int len = 0)
        {
            len = len > 0 ? len / 8 : random.Next(100, 130) / 8;
            var bytes = new byte[len];

            random.NextBytes(bytes);
            bytes[bytes.Length - 1] &= (byte)0x7F;

            return new BigInteger(bytes);
        }

        public static bool IsLikelyPrime(BigInteger number)
        {
            var sieve = EratosthenesSieve.GetSieve();

            foreach (var prime in sieve)
            {
                if (prime >= number)
                    return true;
                if (number % prime == 0)
                    return false;
            }

            return true;
        }

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
                b = b / 2;
            } while (b > 0);

            for (int j = 0; j < steps; j++)
            {
                var a = randNum();
                if (CryptoRSA.EuclidExtended(a, number) > 1)
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
