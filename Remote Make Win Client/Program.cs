using System;



namespace Remote_Make_Win_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine(Prime.GetPrime(100).ToString());
            Console.Read();
        }
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
        private static bool IsPrime(UIntPtr num)
        {
            for (int i = 2; i < (int)num; i++)
                if ((int)num % i == 0)
                    return false;
            return true;
        }
        public static int GetPrime(int digits)
        {
            UIntPtr start = (UIntPtr)(digits * digits);
            while (!IsPrime(start))
                start = (UIntPtr)((int)start+1);
            return (int)start;
        }
    }
}
