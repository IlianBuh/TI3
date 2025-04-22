using System.IO;
using System.Text;

namespace l3.Services.FileViewer;

public class FileViewer
{
    public string ReadDecFile(string p)
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

    public string ReadEncFile(string p)
    {
        var data = File.ReadAllBytes(p);
        StringBuilder s = new StringBuilder(data.Length*4);
        Func <int, int, string> insrtBytes = (int a, int b) => $"{{{a},{b}}},";
        
        byte[] tempa = new byte[4];
        byte[] tempb = new byte[4];
        
        if (data.Length < 10)
        {
            for (int i = 0; i < data.Length; i += sizeof(int)*2)
            {
                
                tempa[0] = data[i];tempa[1] = data[i+1];tempa[2] = data[i+2];tempa[3] = data[i+3];
                tempb[0] = data[i+4];tempb[1] = data[i+5];tempb[2] = data[i+6];tempb[3] = data[i+7];
                s.Append(insrtBytes(BitConverter.ToInt32(tempa), BitConverter.ToInt32(tempb)));
            }   
        }
        else
        {
            for (int i = 0; i < 6*sizeof(int)*2; i += sizeof(int)*2)
            {
                tempa[0] = data[i];tempa[1] = data[i+1];tempa[2] = data[i+2];tempa[3] = data[i+3];
                tempb[0] = data[i+4];tempb[1] = data[i+5];tempb[2] = data[i+6];tempb[3] = data[i+7];
                s.Append(insrtBytes(BitConverter.ToInt32(tempa), BitConverter.ToInt32(tempb)));
            }

            s.Remove(s.Length - 1, 1);
            s.Append("...");
         
            var len = data.Length;
            for (int i = len-6*sizeof(int)*2; i < len; i += sizeof(int)*2)
            {
                tempa[0] = data[i];tempa[1] = data[i+1];tempa[2] = data[i+2];tempa[3] = data[i+3];
                tempb[0] = data[i+4];tempb[1] = data[i+5];tempb[2] = data[i+6];tempb[3] = data[i+7];
                s.Append(insrtBytes(BitConverter.ToInt32(tempa), BitConverter.ToInt32(tempb)));
            }
        }

        s.Remove(s.Length - 1, 1);
        return s.ToString();
    }
}