using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JayDataApi
{
    public abstract class EntityContext
    {
        protected EntityContext(string database)
        {
            InitJayData(database);
        }

        [InlineCode("{this}.jayDataObject = new {this}.constructor.jayDataConstructor({database});")]
        private void InitJayData(string database)
        {
        }

        private dynamic JayDataObject
        {
            [InlineCode("{this}.jayDataObject")]
            get { return null; }
        }

        public Task Ready()
        {
            return Task.FromDoneCallback(JayDataObject, "onReady");
        }
    }
}
