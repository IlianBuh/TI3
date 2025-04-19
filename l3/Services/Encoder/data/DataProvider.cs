using System.IO;
using l3.Encoder.service;

namespace l3.Encoder.data;

public class DataProvider: IDataProvider
{
    StreamReader src;
    StreamWriter dest;
    private const int chunkLength = 4096; 
    public byte[] Fetch()
    {
        byte[] data = new byte[chunkLength];
        int bytesRead = this.src.BaseStream.Read(data, 0, chunkLength);
        
        if (bytesRead < chunkLength)
        {
            byte[] actualData = new byte[bytesRead];
            Buffer.BlockCopy(data, 0, actualData, 0, bytesRead);
            return actualData;
        }
        
        return data;
    }

    public void Save(byte[] data)
    {
        this.dest.BaseStream.Write(data, 0, data.Length);
    }

    public void SetInput(string input)
    {
        this.src = new StreamReader(input);
    }

    public void SetOutput(string output)
    {
        this.dest = new StreamWriter(output);
    }

    public void Stop()
    {   
        this.dest.Close();
        this.src.Close();
    }
}