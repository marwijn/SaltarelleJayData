using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JayDataApi
{
    public class EntityContext
    {
        public EntityContext(string database, string provider)
        {
           InitJayDataConstructorArgument(database, provider); 
        }

        [InlineCode("TESTTESTTEST")]
        public void InitJayDataConstructorArgument(string database, string provider)
        {
        }

        public Task Ready()
        {
            return Task.FromDoneCallback(this, "onReady", new object[0]);
        }
    }
}
