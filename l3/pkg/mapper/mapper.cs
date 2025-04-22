using System.Collections.ObjectModel;

namespace l3.pkg.mapper;

static public class mapper
{
    static public ObservableCollection<T> ListToObservable<T>(List<T> l)
    {
        return new ObservableCollection<T>(l);
    }

    static public byte[] IntsToBytes(int[] ints)
    {
        byte[] bytes = new byte[ints.Length * sizeof(int)];

        Buffer.BlockCopy(ints, 0, bytes, 0, bytes.Length);

        return bytes;
    }
}