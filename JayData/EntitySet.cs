using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JayDataApi
{
    public class EntitySet<T> 
    {
        public dynamic JayDataObject { get; set; }

        public EntitySet()
        {
        }

        public Task<IList<T>> ToList()
        {
            return Task.FromDoneCallback<IList<T>>(JayDataObject, 0, "toArray");
        }
    }
}
