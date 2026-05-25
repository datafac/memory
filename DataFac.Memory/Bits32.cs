using System;
using System.Runtime.CompilerServices;

namespace DataFac.Memory;

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
    private readonly UInt32 _data;

    /// <summary>
    /// Bits data stored as a 32-bit unsigned integer. Each bit can be accessed or modified using the GetBit and SetBit methods.
    /// </summary>
    public uint Data => _data;

    /// <summary>
    /// Constructs an instance from an 32-bit unsigned integer.
    /// </summary>
    /// <param name="data"></param>
    public Bits32(UInt32 data) => _data = data;

    /// <inheritdoc />
    public bool Equals(Bits32 other) => _data == other._data;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Bits32 other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(_data);

    /// <inheritdoc />
    public override string ToString() => $"0x{_data:X8}";

    /// <inheritdoc />
    public static bool operator ==(Bits32 left, Bits32 right) => left.Equals(right);

    /// <inheritdoc />
    public static bool operator !=(Bits32 left, Bits32 right) => !left.Equals(right);

    /// <summary>
    /// Returns the value of the bit at the specified index. Bit indices are zero-based, 
    /// with 0 representing the least significant bit and 31 representing the most significant bit.
    /// </summary>
    /// <param name="index"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GetBit(int index)
    {
        index &= 0x1F; // Ensure index is within 0-31
        UInt32 mask = 1u << index;
        return (_data & mask) != 0;
    }

    /// <summary>
    /// Sets the bit at the specified index to the specified value. Bit indices are zero-based, 
    /// with 0 representing the least significant bit and 31 representing the most significant bit.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bits32 SetBit(int index, bool value)
    {
        index &= 0x1F; // Ensure index is within 0-31
        UInt32 mask = 1u << index;
        UInt32 newData = value ? (_data | mask) : (_data & ~mask);
        return new Bits32(newData);
    }
}
