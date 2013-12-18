using JayDataApi;

namespace SaltarelleJayData.Example
{
    [EntityContext]
    public class Database : EntityContext
    {
        public Database()
            : base("TEST")
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
            entity.BInt = 5;
            var database = new Database();
            var x = database.TheBs;
        }
    }
}
