using System.Runtime.CompilerServices;
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

        public EntitySet<MyEntity> TheBs
        {
            [InlineCode("{this}.$JayDataObject.TheBs")]
            get { return null; }
            [InlineCode("{this}.$JayDataObject.BInt = {value}")]
            set { }
        }
    }

    [Entity]
    public class MyEntity : Entity
    {
        public int BInt
        {
            // [InlineCode("{this}.$JayDataObject.BInt")]
            get;
            // [InlineCode("{this}.$JayDataObject.BInt = {value}")]
            set; }
    }

    class Program
    {
        static void Main()
        {
            var entity = new MyEntity();
            entity.BInt = 7;
            var database = new Database();
            var test = database.TheBs;
        }
    }
}
