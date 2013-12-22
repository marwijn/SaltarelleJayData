using System;
using JayDataApi;
using jQueryApi;

namespace SaltarelleJayData.Example
{
    [EntityContext]
    public class Database : EntityContext
    {
        public Database()
            : base("TEST3")
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

        public int AnotherInt { get; set; }

        public string BString { get; set; }
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
             entity.AnotherInt = 77;
            var database = new Database();
            await database.Ready();
            database.TheBs.Add(entity);
            await database.SaveChanges();
            var entities = await database.TheBs.ToList();

             database.TheBs.Attach(entities[0]);

             entities[0].AnotherInt= 555;
             entities[0].BString = "Hello world" + DateTime.Now.ToLocaleTimeString();

             await database.SaveChanges();

             var database2 = new Database();
             await database2.Ready();
             var entities2 = await database2.TheBs.ToList();

            jQuery.Select("#content").Html(entities2[0].ToString());
        }
    }
}
