namespace l3;

public static class Validator
{
    private const string msgNotPrime = "value is not prime";
    private const string msgOutOfRange = "value is out of range";
    private const string msgNotRelativelyPrime = "values are not relatively prime";

    public static (string, bool) P(int p)
    {
        Func<string, (string, bool)> sendErr = (string msg) => (msg, false);

        if (!isPrime(p))
        {
            return sendErr(msgNotPrime);
        }

        return ("", true);
    }

    public static (string, bool) X(int x, int p)
    {
        Func<string, (string, bool)> sendErr = (string msg) => (msg, false);

        if (x <= 1 || p-1 <= x)
        {
            return sendErr(msgOutOfRange);
        }

        return ("", true);
    }
    
    public static (string, bool) K(int k, int p)
    {
        Func<string, (string, bool)> sendErr = (string msg) => (msg, false);
        const string msgNotRelativelyPrime = $"k and p-1: {Validator.msgNotRelativelyPrime}";

        if (k <= 1 ||  p-1 <= k)
        {
            return sendErr(msgOutOfRange);
        }

        if (!isRelativelyPrime(k, p - 1))
        {
            return sendErr(msgNotRelativelyPrime);
        }

        return ("", true);
    }
    
    private static  bool isPrime(int val)
    {
        if (val <= 1) return false;
        if (val == 2) return true;
        if (val % 2 == 0) return false;

        int boundary = (int)Math.Floor(Math.Sqrt(val));
    
        for (int i = 3; i <= boundary; i += 2)
        {
            if (val % i == 0)
                return false;
        }
    
        return true;
    }

    private static bool isRelativelyPrime(int n, int m)
    {
        return EuclieandEx(n, m) == 1;   
    }

    private static int EuclieandEx(int a, int b)
    {
        int d0 = a, d1=b; 
        int x0=1, x1=0; 
        int y0=0, y1=1;
        int q, d2, x2, y2;
        while (d1 != 0)
        {
            q = d0 / d1;
            d2 = d0 % d1;
            d0 = d1;
            d1 = d2;
            
            x2 = x0 - q * x1;
            x0 = x1;
            x1 = x2;
            
            y2 = y0 - q * y1;
            y0 = y1;
            y1 = y2;
        }
        return d0;
    }
}