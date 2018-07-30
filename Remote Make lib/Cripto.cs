using System;

namespace Remote_Make
{
    namespace Crypto
    {
        class RSA
        {

        }
        class Random
        {
            private static readonly System.Random getrandom = new System.Random();
            public static int GetRandomNumber(int min, int max)
            {
                lock (getrandom) // synchronize
                {
                    return getrandom.Next(min, max);
                }
            }
        }
        class Keys
        {

        }
        class Prime
        {
            private static bool IsPrime(int num)
            {
                for (int i = 2; i < num; i++)
                    if (num % i == 0)
                        return false;
                return true;
            }
            public static int GetPrime(int digits)
            {
                int start = digits * 10;
                for (; IsPrime(start); start++)
                    continue;
                return start;
            }
        }
    }
}
