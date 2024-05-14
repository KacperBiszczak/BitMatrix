using System;
using System.Collections;
using System.Diagnostics.Metrics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Dynamic;
using System.Resources;

namespace BitMatrix;

// prostokątna macierz bitów o wymiarach m x n
public class BitMatrix: IEquatable<BitMatrix>, IEnumerable<int>, ICloneable
{
    #region Properties
    private BitArray data;
    public int NumberOfRows { get; }
    public int NumberOfColumns { get; }
    public bool IsReadOnly => false;
    #endregion

    #region Indexer
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
    #endregion

    #region Constructors
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
    #endregion

    #region BoolToBit, BitToBool
    public static int BoolToBit(bool boolValue) => boolValue ? 1 : 0;
    public static bool BitToBool(int bit) => bit != 0;
    #endregion

    #region ToString
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
    #endregion

    #region IEquatable
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
    #endregion
    #region IEnumerable (Iterator)
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
    #endregion
    #region IClonable
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
    #endregion

    #region Parse
    public static BitMatrix Parse(string s)
    {
        // Check is null
        if(s == null || s.Length < 1) throw new ArgumentNullException("s");

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
                if (cell == 0 || cell == 1)
                {
                    cells[row, col] = cell;
                }
                else
                {
                    throw new FormatException();
                }
            }
        }

        return new BitMatrix(cells);
    }

    public static bool TryParse(string s, out BitMatrix result)
    {
        result = null;

        try
        {
            result = BitMatrix.Parse(s);
        }
        catch(Exception)
        {
            return false;
        }

        return true;
    }
    #endregion

    #region Implicit, Explicit (Ukryty, Jawny)

    public static implicit operator int[,](BitMatrix m)
    {
        int[,] result = new int[m.NumberOfRows, m.NumberOfColumns];

        for (int row = 0; row < m.NumberOfRows; row++)
        {
            for (int col = 0; col < m.NumberOfColumns; col++)
            {
                result[row, col] = m[row, col];
            }
        }

        return result;
    }

    public static explicit operator BitMatrix(int[,] m)
    {
        if (m == null) throw new NullReferenceException();
        if (m.Length < 1) throw new ArgumentOutOfRangeException();

        return new BitMatrix(m);
    }

    public static implicit operator bool[,](BitMatrix m)
    {
        bool[,] result = new bool[m.NumberOfRows, m.NumberOfColumns];

        for (int row = 0;row < m.NumberOfRows; row++)
        {
            for(int col = 0;col < m.NumberOfColumns; col++)
            {
                result[row, col] = Convert.ToBoolean(m[row, col]);
            }
        }

        return result;
    }

    public static explicit operator BitMatrix(bool[,] m)
    {
        if (m == null) throw new NullReferenceException();
        if (m.Length < 1) throw new ArgumentOutOfRangeException();

        return new BitMatrix(m);
    }

    public static explicit operator BitArray(BitMatrix m)
    {
        int length = m.NumberOfRows * m.NumberOfColumns;
        int counter = 0;

        BitArray arr = new BitArray(length);

        foreach(var bit in (bool[,])m)
        {
            if(counter < length)
            {
                arr[counter++] = bit;
            }
        }

        return arr;
    }

    #endregion

    #region BitOperations

    public BitMatrix And(BitMatrix other)
    {
        if(other == null) throw new ArgumentNullException();
        if (NumberOfRows != other.NumberOfRows || NumberOfColumns != other.NumberOfColumns) throw new ArgumentException();

        for (int row = 0; row < NumberOfRows; row++)
        {
            for (int col = 0; col < NumberOfColumns; col++)
            {
                if (this[row, col] == 1 && other[row, col] == 1)
                    this[row, col] = 1;
                else
                    this[row, col] = 0;
            }
        }

        return this;
    }

    public BitMatrix Or(BitMatrix other)
    {
        if (other == null) throw new ArgumentNullException();
        if (NumberOfRows != other.NumberOfRows || NumberOfColumns != other.NumberOfColumns) throw new ArgumentException();

        for (int row = 0; row < NumberOfRows; row++)
        {
            for (int col = 0; col < NumberOfColumns; col++)
            {
                if (this[row, col] == 1 || other[row, col] == 1)
                    this[row, col] = 1;
                else
                    this[row, col] = 0;
            }
        }

        return this;
    }
    
    public BitMatrix Xor(BitMatrix other)
    {
        if (other == null) throw new ArgumentNullException();
        if (NumberOfRows != other.NumberOfRows || NumberOfColumns != other.NumberOfColumns) throw new ArgumentException();

        for (int row = 0; row < NumberOfRows; row++)
        {
            for (int col = 0; col < NumberOfColumns; col++)
            {
                if (this[row, col] != other[row, col])
                    this[row, col] = 1;
                else
                    this[row, col] = 0;
            }
        }

        return this;
    }

    public BitMatrix Not()
    {
        for (int row = 0; row < NumberOfRows; row++)
        {
            for (int col = 0; col < NumberOfColumns; col++)
            {
                if (this[row, col] == 1)
                    this[row, col] = 0;
                else
                    this[row, col] = 1;
            }
        }

        return this;
    }

    #endregion

    #region Operators 
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

    public static BitMatrix operator &(BitMatrix left, BitMatrix right)
    {
        if (left == null || right == null) throw new ArgumentNullException();

        BitMatrix leftClone = (BitMatrix)left.Clone();
        return leftClone.And(right);
    }

    public static BitMatrix operator |(BitMatrix left, BitMatrix right)
    {
        if (left == null || right == null) throw new ArgumentNullException();

        BitMatrix leftClone = (BitMatrix)left.Clone();
        return leftClone.Or(right);
    }

    public static BitMatrix operator ^(BitMatrix left, BitMatrix right)
    {
        if (left == null || right == null) throw new ArgumentNullException();

        BitMatrix leftClone = (BitMatrix)left.Clone();
        return leftClone.Xor(right);
    }

    public static BitMatrix operator !(BitMatrix left)
    {
        if (left == null) throw new ArgumentNullException();

        BitMatrix leftClone = (BitMatrix)left.Clone();
        return leftClone.Not();
    }
    #endregion
}