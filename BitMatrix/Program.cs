namespace BitMatrix
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //BitMatrix b1 = new BitMatrix(2, 2, new int[] { 1, 0, 0, 1, 1 });
            //Console.WriteLine(b1[1, 1]);

            //int[] arr1 = { 1, 0, 0, 4, 0, 6 };
            //BitMatrix b2 = new BitMatrix(2, 2, arr1);
            //Console.WriteLine(b2);

            //int[,] arr2 = { { 1, 0, 3 }, { 0, 1, 2 } };
            //BitMatrix b3 = new BitMatrix(arr2);
            //Console.WriteLine(b3);


            //bool[,] arr3 = { { true, true, false }, { false, true, true } };
            //BitMatrix b4 = new BitMatrix(arr3);
            //Console.WriteLine(b4);

            //int[] arr4 = { };
            //BitMatrix b5 = new BitMatrix(2, 2, null);
            //Console.WriteLine(b5);

            //Console.WriteLine(b1 == b3);

            //Console.WriteLine("Iterator test:");
            //foreach(var item in b3)
            //{
            //    Console.Write(item + " ");
            //}

            // indekser - indeksy poza zakresem
            //int[] arr = new int[] { -1, 1, 3, 4 };
            //foreach (var i in arr)
            //    foreach (var j in arr)
            //    {
            //        var m = new BitMatrix(3, 4);
            //        try
            //        {
            //            m[i, j] = 1;
            //            Console.WriteLine($"m[{i}, {j}] = {m[i, j]}");
            //        }
            //        catch (IndexOutOfRangeException)
            //        {
            //            Console.WriteLine($"m[{i}, {j}] = exception");
            //        }
            //    }

            //int[,] arr = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            //BitMatrix m = new BitMatrix(arr);
            //BitMatrix m1 = (BitMatrix)m.Clone();

            //Console.WriteLine(m);
            //Console.WriteLine(m1);

            //m[0, 0] = 0;

            //Console.WriteLine(m);
            //Console.WriteLine(m1);

            // Parse, dane poprawne
            string s = @"1111
                        0000
                        1100";
            Console.WriteLine(BitMatrix.Parse(s));
        }
    }
}
