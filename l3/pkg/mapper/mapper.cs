using System.Collections.ObjectModel;

namespace l3.pkg.mapper;

static public class mapper
{
    static public ObservableCollection<T> ListToObservable<T>(List<T> l)
    {
        return new ObservableCollection<T>(l);
    }
}