namespace l3.Encoder.service;

public interface IEncoder
{
    public string Encode(int p, int g, int y, int k,  string dest, string src);
    public void Decode(int p, int x,  string dest, string src);
    public List<int> ListPrimitiveRoot(int p);
    public int CalcY(int g, int x, int p);
}