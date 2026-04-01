using System;
using System.Runtime.CompilerServices;

namespace DataFac.Memory;

/// <summary>
/// Represents a 64-bit unsigned integer that provides methods for querying and manipulating individual bits in an
/// immutable manner.
/// </summary>
/// <remarks>This struct is immutable; all operations that modify bits return a new instance. Bit indices
/// are zero-based and must be in the range 0 to 63, inclusive. Attempting to access or modify a bit outside this
/// range will result in an ArgumentOutOfRangeException. Bits64 is useful for scenarios where efficient, type-safe
/// bit manipulation is required, such as flags, masks, or low-level protocol handling.</remarks>
public readonly struct Bits64 : IEquatable<Bits64>
{
    public readonly UInt64 Data;
    public Bits64(UInt64 data) => Data = data;
    public Bits64(Bits64 other) => Data = other.Data;
    public bool Equals(Bits64 other) => Data == other.Data;
    public override bool Equals(object? obj) => obj is Bits64 other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Data);
    public override string ToString() => $"0x{Data:X8}";
    public static bool operator ==(Bits64 left, Bits64 right) => left.Equals(right);
    public static bool operator !=(Bits64 left, Bits64 right) => !left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GetBit(int index)
    {
        index &= 0x3F; // Ensure index is within 0-63
        UInt64 mask = 1u << index;
        return (Data & mask) != 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bits64 SetBit(int index, bool value)
    {
        index &= 0x3F; // Ensure index is within 0-63
        UInt64 mask = 1uL << index;
        UInt64 newData = value ? (Data | mask) : (Data & ~mask);
        return new Bits64(newData);
    }
}