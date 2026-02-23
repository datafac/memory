using System;

namespace DataFac.Memory
{
    /// <summary>
    /// Represents a 32-bit unsigned integer that provides methods for querying and manipulating individual bits in an
    /// immutable manner.
    /// </summary>
    /// <remarks>This struct is immutable; all operations that modify bits return a new instance. Bit indices
    /// are zero-based and must be in the range 0 to 31, inclusive. Attempting to access or modify a bit outside this
    /// range will result in an ArgumentOutOfRangeException. Bits32 is useful for scenarios where efficient, type-safe
    /// bit manipulation is required, such as flags, masks, or low-level protocol handling.</remarks>
    public readonly struct Bits32 : IEquatable<Bits32>
    {
        public readonly UInt32 Data;
        public Bits32(UInt32 data) => Data = data;
        public Bits32(Bits32 other) => Data = other.Data;
        public bool Equals(Bits32 other) => Data == other.Data;
        public override bool Equals(object? obj) => obj is Bits32 other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Data);
        override public string ToString() => $"0x{Data:X8}";
        public static bool operator ==(Bits32 left, Bits32 right) => left.Equals(right);
        public static bool operator !=(Bits32 left, Bits32 right) => !left.Equals(right);

        public bool GetBit(int index)
        {
            if (index < 0 || index > 31) throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 31.");
            UInt32 mask = 1u << index;
            return (Data & mask) != 0;
        }

        public Bits32 SetBit(int index, bool value)
        {
            if (index < 0 || index > 31) throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 31.");
            UInt32 mask = 1u << index;
            UInt32 newData = value ? (Data | mask) : (Data & ~mask);
            return new Bits32(newData);
        }
    }
}