using System.Runtime.CompilerServices;

namespace JayDataApi
{
    public class EntitySet<T> : AsyncQueryable<T> where T : Entity, new ()
    {
        public EntitySet(object jayDataObject) : base (jayDataObject)
        {
        }

        [InlineCode("{this}.jayDataObject.add({entity}.jayDataObject)")]
        public void Add(T entity)
        {
        }

        [InlineCode("{this}.jayDataObject.attach({entity}.jayDataObject)")]
        public void Attach (T entity)
        {
        }
        
        [InlineCode("{this}.jayDataObject.remove({entity}.jayDataObject)")]
        public void Remove(T entity)
        {
        }
    }
}
