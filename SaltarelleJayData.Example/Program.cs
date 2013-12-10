namespace SaltarelleJayData.Example
{
    public class A
    {
        public A(int a)
        {
            AInt = a;
        }

        public int AInt { get; set; }
    }

    public class B : A
    {
        public B(int a, int b) : base(a)
        {
            BInt = b;
        }

        public int BInt { get; set; }
    }

    class Program
    {
        static void Main()
        {
            var b = new B(2, 3);
        }
    }
}
