using System.Threading.Tasks;

namespace JayDataApi
{
    public class EntityContext
    {
        public Task Ready()
        {
            return Task.FromDoneCallback(this, "onReady", new object[0]);
        }
    }
}
