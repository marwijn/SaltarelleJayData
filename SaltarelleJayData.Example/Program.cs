using JayDataApi;
using jQueryApi;

namespace SaltarelleJayData.Example
{
    [EntityContext]
    public class Database : EntityContext
    {
        public Database()
            : base("TEST")
        {
            dynamic self = this;
            TheBs = new EntitySet<MyEntity>(self.jayDataObject.TheBs);
        }

        public EntitySet<MyEntity> TheBs { get; set; }
    }

    [Entity]
    public class MyEntity : Entity
    {
        [Key]
        [Computed]
        public int BInt { get; set; }
    }

    class Program
    {
        static void Main()
        {
            jQuery.OnDocumentReady(Run);
        }

         static async void Run()
        {
            var entity = new MyEntity ();
            var database = new Database();
            await database.Ready();
            database.TheBs.Add(entity);
            await database.SaveChanges();
            var entities = await database.TheBs.ToList();
            var z = entities.Count;

             var x = 10;
             var y = z.ToString();

            jQuery.Select("#content").Html(z.ToString());
        }
    }
}
