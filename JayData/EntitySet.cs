using System.Runtime.CompilerServices;

namespace JayDataApi
{
    public class EntitySet<T> 
    {
        private dynamic JayDataObject
        {
            [InlineCode("{this}.jayDataObject")]
            get { return null; }
        }
    }
}
