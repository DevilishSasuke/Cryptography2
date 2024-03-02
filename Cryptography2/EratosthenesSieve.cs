namespace Cryptography
{
    public static class EratosthenesSieve
    {
        private static int previousN;
        private static int[] sieve = Array.Empty<int>();
        public static int[] GetSieve(int n = 5000)
        {
            if (n < 1)
                n = 5000;
            if (sieve.Length > 0 && previousN == n)
                return sieve;
            previousN = n;

            var methodSieve = new int[n];
            for (int i = 0; i < methodSieve.Length; ++i)
                methodSieve[i] = i;

            for (int j = 2; j < methodSieve.Length; ++j)
            {
                if (methodSieve[j] == 0) continue;
                for (int k = j * j; k < methodSieve.Length; k += j)
                    methodSieve[k] = 0;
            }

            var primeList = new List<int>();
            for (int i = 2; i < methodSieve.Length; ++i)
                if (methodSieve[i] != 0)
                    primeList.Add(methodSieve[i]);

            sieve = primeList.ToArray();
            return sieve;
        }
    }
}
