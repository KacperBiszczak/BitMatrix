using System;
using System.Collections;
using System.Diagnostics.Metrics;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace BitMatrix;

// prostokątna macierz bitów o wymiarach m x n
public class BitMatrix: IEquatable<BitMatrix>, IEnumerable<int>, ICloneable
{
    private BitArray data;
    public int NumberOfRows { get; }
    public int NumberOfColumns { get; }
    public bool IsReadOnly => false;

    // Indexer
    public int this[int row, int column]
    {
        get
        {
            if(row < 0 || column < 0 || row >= NumberOfRows || column >= NumberOfColumns)
                throw new IndexOutOfRangeException();

            int index = row * NumberOfColumns + column;
            return BoolToBit(data[index]);
        }

        set
        {
            if (row < 0 || column < 0 || row >= NumberOfRows || column >= NumberOfColumns)
                throw new IndexOutOfRangeException();

            int index = row * NumberOfColumns + column;
            data[index] = BitToBool(value);
        }
    }

    // tworzy prostokątną macierz bitową wypełnioną `defaultValue`
    public BitMatrix(int numberOfRows, int numberOfColumns, int defaultValue = 0)
    {
        if (numberOfRows < 1 || numberOfColumns < 1)
            throw new ArgumentOutOfRangeException("Incorrect size of matrix");
        data = new BitArray(numberOfRows * numberOfColumns, BitToBool(defaultValue));
        NumberOfRows = numberOfRows;
        NumberOfColumns = numberOfColumns;
    }

    public BitMatrix(int numberOfRows, int numberOfColumns, params int[] bits) : this(numberOfRows, numberOfColumns, 0)
    {
        if(bits != null)
        {
            if (bits.Length > 0)
            {
                for (int i = 0; i < bits.Length; i++)
                {
                    if (i < bits.Length && i < data.Length)
                        data[i] = BitToBool(bits[i]);
                }
            }
        }
    }

    public BitMatrix(int[,] bits) : this(bits.GetLength(0), bits.GetLength(1))
    {
        if (bits == null) throw new NullReferenceException();
        if (bits.Length == 0) throw new ArgumentOutOfRangeException();

        int counter = 0;
        foreach (var bit in bits)
        {
            if (counter < bits.Length && counter < data.Length)
                data[counter++] = BitToBool(bit);
        }
    }

    public BitMatrix(bool[,] bits) : this(bits.GetLength(0), bits.GetLength(1))
    {
        if (bits == null) throw new NullReferenceException();
        if (bits.Length == 0) throw new ArgumentOutOfRangeException();

        int counter = 0;
        foreach (var bit in bits)
        {
            if(counter < bits.Length && counter < data.Length)
                data[counter++] = bit;
        }
    }

    public static int BoolToBit(bool boolValue) => boolValue ? 1 : 0;
    public static bool BitToBool(int bit) => bit != 0;

    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < data.Count; i++)
        {
            result += BoolToBit(data[i]).ToString();
            
            if (i%NumberOfColumns == NumberOfColumns-1)
            {
                result += "\r\n";
            }
        }

        return result;
    }

    public bool Equals(BitMatrix? other)
    {
        if (other == null)
            return false;

        if (NumberOfRows != other.NumberOfRows || NumberOfColumns != other.NumberOfColumns)
            return false;

        for (int i = 0; i < other.data.Length; i++)
        {
            if (data[i] != other.data[i]) 
                return false;
        }

        return true;
    }

    public override bool Equals(object other)
    {
        if (other == null || !(other.GetType() == typeof(BitMatrix)))
            return false;

        return Equals((BitMatrix)other);
    }

    public override int GetHashCode()
    {
        return data.GetHashCode();
    }

    // Iterator
    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 0; i < NumberOfRows; i++)
        {
            for (int j = 0; j < NumberOfColumns; j++)
            {
                yield return BoolToBit(data[i * NumberOfColumns + j]);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public object Clone()
    {
        var clonedMatrix = new BitMatrix(NumberOfRows, NumberOfColumns);
        for(int row = 0; row < NumberOfRows; row++)
        {
            for(int col = 0; col < NumberOfColumns; col++)
            {
                clonedMatrix[row, col] = this[row, col];
            }
        }

        return clonedMatrix;
    }

    public static BitMatrix Parse(string s)
    {
        // Check is null
        if(s == null) throw new ArgumentNullException("s");

        string[] rows = s.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        // Remove white-spaces rows
        for(int i =  0; i < rows.Length; i++)
        {
            rows[i] = rows[i].Trim();
        }

        // Checking format
        foreach (var row in rows)
        {
            if (rows[0].Length != row.Length)
                throw new FormatException();
        }
       
        int[,] cells = new int[rows.Length, rows[0].Trim().Length];
        int cell = 0;

        for(int row = 0; row < rows.Length; row++)
        {
            for (int col = 0; col < rows[0].Length; col++)
            {
                cell = int.Parse(rows[row][col].ToString());
                if (cell != 0 || cell != 1)
                    cells[row, col] = cell;
            }
        }

        return new BitMatrix(cells);
    }

    public static bool operator ==(BitMatrix left, BitMatrix right)
    {
        if (ReferenceEquals(left, null))
            return ReferenceEquals(right, null);
    
        return left.Equals(right);
    }

    public static bool operator !=(BitMatrix left, BitMatrix right)
    {
        return !(left == right);
    }
}