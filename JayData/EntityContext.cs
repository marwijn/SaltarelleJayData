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

        [InlineCode("{this}.$jayDataObject = new {this}.constructor.$jayDataConstructor({database});")]
        private void InitJayData(string database)
        {
        }

        public Task Ready()
        {
            return Task.FromDoneCallback(this, "onReady", new object[0]);
        }
    }
}
