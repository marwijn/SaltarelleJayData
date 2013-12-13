using JayDataApi;

namespace SaltarelleJayData.Example
{
    public class Database : EntityContext
    {
        public Database() : base ("db", "provider")
        {
        }

        public EntitySet<MyEntity> TheBs { get; set; }
    }

    [Entity]
    public class MyEntity : Entity
    {
        public int BInt { get; set; }
    }

    class Program
    {
        static void Main()
        {
            var entity = new MyEntity();
            var database = new Database();
        }
    }
}
