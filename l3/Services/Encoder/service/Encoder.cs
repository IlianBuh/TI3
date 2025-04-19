using System.Windows;
using System.Windows.Controls;
using l3.Encoder.transport;

namespace l3.Encoder.service;

public class Encoder: IEncoder
{
    private IDataProvider dataPrvdr;
    public Encoder(IDataProvider dataProvider)
    {
        this.dataPrvdr = dataProvider;
    }
    public void Encode(int p, int g, int y, int k,  string dest, string src)
    {
        this.setDataPrvdr(dest, src);

        byte[] data;
        data = this.dataPrvdr.Fetch();
        byte[] output = new byte[data.Length * 2];
        int idx;
        byte a, b;
        while (data.Length > 0)
        {
            idx = 0;
            foreach (byte d in data)
            {
                (a, b) = this.encodeByte(d, g, y, k, p);
                output[idx++] = a;
                output[idx++] = b;
            }
            
            this.dataPrvdr.Save(output);
            data = this.dataPrvdr.Fetch();
        }
        
        this.dataPrvdr.Stop();
    }

    public void Decode(int p, int x,  string dest, string src)
    {
        this.setDataPrvdr(dest, src);
        
        byte[] data;
        data = this.dataPrvdr.Fetch();
        int idx = 0, len = data.Length;
        byte[] output = new byte[len / 2];
        byte a, b;
        while (data.Length > 0)
        {
            
            for (int i = 0;i < len;i += 2)
            {
                output[i/2] = this.decodeByte(data[i], data[i+1], x, p);
            }
            
            this.dataPrvdr.Save(output);
            data = this.dataPrvdr.Fetch();
        }
        
        this.dataPrvdr.Stop();
    }

    public List<int> ListPrimitiveRoot(int p)
    {
        var list = new List<int>();
        const int start = 2;
        int end = p - 1;
        
        var divisors = getPrimeDivisors(end); 
        
        for (int g = start; g < end; g++)
        {
            if (this.isPrimitiveRoot(g, p, divisors))
            {
                list.Add(g);
            }
        } 
        
        return list;
    }

    public int CalcY(int g, int x, int p)
    {
        return fastExp(g, x, p);
    }

    private List<int> getPrimeDivisors(int val)
    {   
        var list = new List<int>();
        if (val % 2 == 0)
        {
            list.Add(2);
            while (val % 2 == 0)
            {
                val /= 2;
            }
        }

        var d = 1;
        while (val > 1)
        {
            d += 2;
            if (val % d == 0)
            {
                list.Add(d);
                while (val % d == 0)
                {
                    val /= d;
                }
            }
        }
        
        return list;
    }

    private bool isPrimitiveRoot(int g, int p, List<int> divisors)
    {
        var end = p - 1;
        foreach (var divisor in divisors)
        {
            if (fastExp(g, end/divisor, p) == 1)
            {
                return false;
            }
        }

        return true;
    }

    private int fastExp(int a, int z, int module)
    {
        int x = 1;
        while (z > 0)
        {
            while (z % 2 == 0)
            {
                z /= 2;
                a = (a * a) % module;
            }

            z--;
            x = (x * a) % module;
        }

        return x;
    }

    private (byte, byte) encodeByte(int m, int g, int y, int k, int p)
    {
        byte a = (byte)fastExp(g, k, p);
        byte b = (byte)((fastExp(y, k, p) * (m % p)) % p);
        return (a, b);
    }

    private void setDataPrvdr(string dest, string src)
    {
        this.dataPrvdr.SetInput(src);
        this.dataPrvdr.SetOutput(dest);
    }

    private byte decodeByte(byte a, byte b, int x, int p)
    {
        return (byte)(((b % p) * fastExp(a, p - x - 1, p)) % p);
    }
}