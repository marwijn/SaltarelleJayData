using System.Runtime.CompilerServices;

namespace JayDataApi
{
    public abstract class Entity
    {
        protected Entity()
        {
           InitJayData();
        }

        [InlineCode("{this}.jayDataObject = new {this}.constructor.jayDataConstructor();")]
        private void InitJayData()
        {
        }
    }
}
