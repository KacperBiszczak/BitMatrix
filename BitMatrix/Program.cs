namespace BitMatrix
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BitMatrix b1 = new BitMatrix(2,2,new int[] { 1, 0, 0, 1, 1 });
            Console.WriteLine(b1);

            int[] arr1 = { 1, 0, 3, 4, 0, 6 };
            BitMatrix b2 = new BitMatrix(5, 6, arr1);
            Console.WriteLine(b2);

            int[,] arr2 = { { 1, 0, 3 }, { 0, 1, 2 } };
            BitMatrix b3 = new BitMatrix(arr2);
            Console.WriteLine(b3);


            bool[,] arr3 = { { true, true, false }, { false, true, true } };
            BitMatrix b4 = new BitMatrix(arr3);
            Console.WriteLine(b4);

            int[] arr4 = { };
            BitMatrix b5 = new BitMatrix(2,2,null);
            Console.WriteLine(b5);
        }
    }
}
