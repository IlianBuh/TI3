using l3.Encoder.service;

namespace l3.Encoder.transport;

public class API
{
    private IEncoder encoder;

    public API(IEncoder encoder)
    {
        this.encoder = encoder;
    }

    public string Encode(int p,  int g, int y, int k, string dest, string src)
    {
        string msg;
        bool valid;
        Func<string, string> sendErr = (string msg) =>  $"Failed to start encoding.\n{msg}";
        
        (msg, valid) = this.validateEncodeInput(p, k);
        if (!valid)
        {
            return sendErr(msg);
        }

        this.encoder.Encode(p, g, y, k, dest, src);
        
        return "";
    }
    public string Decode(int p, int x, string dest, string src)
    {
        string msg;
        bool valid;
        Func<string, string> sendErr = (string msg) =>  $"Failed to start encoding.\n{msg}";
        
        (msg, valid) = this.validateDecodeInput(p, x);
        if (!valid)
        {
            return sendErr(msg);
        }

        this.encoder.Decode(p, x, dest, src);
        
        return "";
    }

    public (List<int>, string) FetchPrimitiveG(int p)
    {
        string msg;
        bool valid;
        Func<string, (List<int>, string)> sendErr = (string msg) => (null, $"Failed to start encoding.\n{msg}");
        
        (msg, valid) = Validator.P(p);
        if (!valid)
        {
            return sendErr($"P: {msg}");
        }
        
        return (this.encoder.ListPrimitiveRoot(p), "");
    }

    public int CalcY(int g, int x, int p)
    {
        return this.encoder.CalcY(g, x, p);
    }

    private (string, bool) validateEncodeInput(int p, int k)
    {
        string msg;
        bool valid;
        Func<string, (string, bool)> sendErr = (string msg) => (msg, false);
        
        (msg, valid) = Validator.P(p);
        if (!valid)
        {
            return sendErr($"P: {msg}");
        }

        (msg, valid) = Validator.K(k, p);
        if (!valid)
        {
            return sendErr($"K: {msg}");
        }

        return ("", true);
    }
    
    private (string, bool) validateDecodeInput(int p, int x)
    {
        string msg;
        bool valid;
        Func<string, (string, bool)> sendErr = (string msg) => (msg, false);
        
        (msg, valid) = Validator.P(p);
        if (!valid)
        {
            return sendErr($"P: {msg}");
        }

        (msg, valid) = Validator.X(x, p);
        if (!valid)
        {
            return sendErr($"X: {msg}");
        }

        return ("", true);
    }
}