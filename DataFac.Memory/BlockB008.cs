using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct BlockB008 : IMemBlock, IEquatable<BlockB008>
    {
        [FieldOffset(0)] public BlockB004 A;
        [FieldOffset(4)] public BlockB004 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, 8), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, 8),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif

        public bool Equals(BlockB008 other) => _long == other._long;

        [FieldOffset(0)] public long _long;
        public long Int64ValueLE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return _long;
                else
                    return BinaryPrimitives.ReverseEndianness(_long);
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _long = value;
                else
                    _long = BinaryPrimitives.ReverseEndianness(value);
            }
        }
        public long Int64ValueBE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return BinaryPrimitives.ReverseEndianness(_long);
                else
                    return _long;
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _long = BinaryPrimitives.ReverseEndianness(value);
                else
                    _long = value;
            }
        }

        [FieldOffset(0)] public double _double;
        public double DoubleValueLE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return _double;
                else
                    return BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(_long));
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _double = value;
                else
                    _long = BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(value));
            }
        }
        public double DoubleValueBE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(_long));
                else
                    return _double;
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _long = BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(value));
                else
                    _double = value;
            }
        }
    }

}