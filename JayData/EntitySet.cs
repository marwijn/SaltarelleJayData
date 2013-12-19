using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JayDataApi
{
    public class EntitySet<T> where T : Entity
    {
        public EntitySet(dynamic jayDataObject)
        {
            JayDataObject = jayDataObject;
        }

        private dynamic JayDataObject
        {
            [InlineCode("{this}.jayDataObject")]
            get { return null; }
            [InlineCode("{this}.jayDataObject={value}")]
            set { }
        }

        public Task<IList<T>> ToList()
        {
            return Task.FromDoneCallback<IList<T>>(JayDataObject, "toArray");
        }

        [InlineCode("{this}.jayDataObject.add({entity}.jayDataObject)")]
        public void Add(T entity)
        {
        }
    }
}
