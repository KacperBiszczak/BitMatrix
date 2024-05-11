using System.Collections;

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

            // Parse, string pusty

            //// konwersja `BitMatrix` na `BitArray`
            //var m = new BitMatrix(2, 3, 1, 0, 1, 1, 1, 0);
            //BitArray bitArr = (BitArray)m;

            //Console.WriteLine(m.NumberOfRows * m.NumberOfColumns == bitArr.Count);

            //for (int i = 0; i < m.NumberOfRows; i++)
            //    for (int j = 0; j < m.NumberOfColumns; j++)
            //        if (m[i, j] != BitMatrix.BoolToBit(bitArr[i * m.NumberOfColumns + j]))
            //        {
            //            Console.WriteLine("Fail");
            //            return;
            //        }

            //// czy niezależna kopia
            //m[1, 2] = 1;
            //Console.WriteLine(m[1, 2] != BitMatrix.BoolToBit(bitArr[5]));

            // funkcja `And`
            // dane poprawne
            var m1 = new BitMatrix(2, 3, 1, 0, 1, 1, 1, 0);
            var m2 = new BitMatrix(2, 3, 1, 1, 0, 1, 1, 0);
            var expected = new BitMatrix(2, 3, 1, 0, 0, 1, 1, 0);
            var m3 = m1.And(m2);
            // czy poprawnie And i czy zwraca `this`
            if (expected.Equals(m1) && ReferenceEquals(m1, m3))
                Console.WriteLine("Correct data: Pass");

            // argument `null`
            try
            {
                m2.And(null);
                Console.WriteLine("Argument null: Fail");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Argument null: Pass");
            }

            // niepoprawne wymiary
            try
            {
                m2.And(new BitMatrix(2, 1));
                Console.WriteLine("Incorrect dimensions: Fail");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Incorrect dimensions: Pass");
            }
        }
    }
}
