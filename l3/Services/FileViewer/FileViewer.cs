using System.IO;
using System.Text;

namespace l3.Services.FileViewer;

public class FileViewer
{
    public string ReadEncFile(string p)
    {
        var bytes = File.ReadAllBytes(p);
        StringBuilder s = new StringBuilder(bytes.Length*4);
        if (bytes.Length < 10)
        {
            foreach (var b in bytes)
            {
                s.Append(b.ToString("D3"));
                s.Append(",");
            }   
        }
        else
        {
            for (int i = 0; i < 5; i ++)
            {
                s.Append(bytes[i].ToString("D3"));
                s.Append(",");
            }

            s.Remove(s.Length - 1, 1);
            s.Append("...");
         
            var len = bytes.Length;
            for (int i = len-6; i < len; i ++)
            {
                s.Append(bytes[i].ToString("D3"));
                s.Append(",");
            }
        }

        s.Remove(s.Length - 1, 1);
        return s.ToString();
    }
}