using System;
using System.Collections;
using System.Diagnostics.Metrics;

namespace BitMatrix;

// prostokątna macierz bitów o wymiarach m x n
public class BitMatrix: IEquatable<BitMatrix>
{
    private BitArray data;
    public int NumberOfRows { get; }
    public int NumberOfColumns { get; }
    public bool IsReadOnly => false;

    public int this[int row, int column]
    {
        get
        {
            int index = row * NumberOfRows + column;
            return BoolToBit(data[index]);
        }

        set
        {
            //data[row * NumberOfRows + column] = value;
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