namespace l3.Encoder.service;

public interface IDataProvider
{
    public byte[] Fetch();
    public void Save(byte[] data);
    public void SetInput(string input);
    public void SetOutput(string output);
    public void Stop();
}