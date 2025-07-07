using DataFac.UnsafeHelpers;
using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct BlockB008 : IMemBlock, IEquatable<BlockB008>
    {
        private const int Size = 8;

        public int BlockSize => Size;

        [FieldOffset(0)] public BlockB004 A;
        [FieldOffset(4)] public BlockB004 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, Size), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, Size),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif

        public string UTF8String
        {
            get => BlockHelper.GetString(ref this);
            set => BlockHelper.SetString(ref this, value);
        }

        public bool IsEmpty => _long == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BlockB008 other) => _long == other._long;

        public override bool Equals(object? obj) => obj is BlockB008 other && Equals(other);
        public override int GetHashCode()
        {
            var self = BlockHelper.AsReadOnlySpan(ref this);
            HashCode hashCode = new HashCode();
            hashCode.Add(self.Length);
#if NET8_0_OR_GREATER
            hashCode.AddBytes(self);
#else
            for (int i = 0; i < self.Length; i++)
            {
                hashCode.Add(self[i]);
            }
#endif
            return hashCode.ToHashCode();
        }

        public PairOfInt32 PairOfInt32LE
        {
            get => new PairOfInt32(A.Int32ValueLE, B.Int32ValueLE);
            set
            {
                A.Int32ValueLE = value.A;
                B.Int32ValueLE = value.B;
            }
        }

        public PairOfInt32 PairOfInt32BE
        {
            get => new PairOfInt32(A.Int32ValueBE, B.Int32ValueBE);
            set
            {
                A.Int32ValueBE = value.A;
                B.Int32ValueBE = value.B;
            }
        }

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

        [FieldOffset(0)] public ulong _ulong;
        public ulong UInt64ValueLE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return _ulong;
                else
                    return BinaryPrimitives.ReverseEndianness(_ulong);
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _ulong = value;
                else
                    _ulong = BinaryPrimitives.ReverseEndianness(value);
            }
        }
        public ulong UInt64ValueBE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return BinaryPrimitives.ReverseEndianness(_ulong);
                else
                    return _ulong;
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _ulong = BinaryPrimitives.ReverseEndianness(value);
                else
                    _ulong = value;
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