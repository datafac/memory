using DataFac.UnsafeHelpers;
using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct BlockB016 : IMemBlock, IEquatable<BlockB016>
    {
        private const int Size = 16;

        public int BlockSize => Size;

        [FieldOffset(0)] public BlockB008 A;
        [FieldOffset(8)] public BlockB008 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, Size), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, Size),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);

#endif

        public void WriteTo(Span<byte> target) => BlockHelper.AsReadOnlySpan(ref this).CopyTo(target);
        public void WriteTo(int start, int length, Span<byte> target) => BlockHelper.AsReadOnlySpan(ref this).Slice(start, length).CopyTo(target);

        public string ToBase64String()
        {
            var span = BlockHelper.AsReadOnlySpan(ref this);
#if NET8_0_OR_GREATER
            return Convert.ToBase64String(span);
#else
            return Convert.ToBase64String(span.ToArray());
#endif
        }

        public string ToBase64String(int start, int length)
        {
            var span = BlockHelper.AsReadOnlySpan(ref this).Slice(start, length);
#if NET8_0_OR_GREATER
            return Convert.ToBase64String(span);
#else
            return Convert.ToBase64String(span.ToArray());
#endif
        }

        public string UTF8String
        {
            get => BlockHelper.GetString(ref this);
            set => BlockHelper.SetString(ref this, value);
        }

        public bool IsEmpty => BlockHelper.AsReadOnlySpanOfInt64(ref this).AreAllZero();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BlockB016 other) => other._guid == _guid;
        public override bool Equals(object? obj) => obj is BlockB016 other && Equals(other);
        public override int GetHashCode() => _guid.GetHashCode();

        public PairOfInt64 PairOfInt64LE
        {
            get => new PairOfInt64(A.Int64ValueLE, B.Int64ValueLE);
            set
            {
                A.Int64ValueLE = value.A;
                B.Int64ValueLE = value.B;
            }
        }

        public PairOfInt64 PairOfInt64BE
        {
            get => new PairOfInt64(A.Int64ValueBE, B.Int64ValueBE);
            set
            {
                A.Int64ValueBE = value.A;
                B.Int64ValueBE = value.B;
            }
        }

        [FieldOffset(0)] public Guid _guid;
        public Guid GuidValueLE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return _guid;
                else
                {
                    var span = BlockHelper.AsReadOnlySpan(ref this);
                    Guid result = GuidHelper.ReadFromSpan(span, false);
                    return result;
                }
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _guid = value;
                else
                {
                    var span = BlockHelper.AsWritableSpan(ref this);
                    GuidHelper.WriteToSpan(span, false, value);
                }
            }
        }

        public Guid GuidValueBE
        {
            get
            {
                if (!BitConverter.IsLittleEndian)
                    return _guid;
                else
                {
                    var span = BlockHelper.AsReadOnlySpan(ref this);
                    Guid result = GuidHelper.ReadFromSpan(span, true);
                    return result;
                }
            }
            set
            {
                if (!BitConverter.IsLittleEndian)
                    _guid = value;
                else
                {
                    var span = BlockHelper.AsWritableSpan(ref this);
                    GuidHelper.WriteToSpan(span, true, value);
                }
            }
        }

#if NET8_0_OR_GREATER
        [FieldOffset(0)] public Int128 _int128;
        public Int128 Int128ValueLE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return _int128;
                else
                    return BinaryPrimitives.ReverseEndianness(_int128);
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _int128 = value;
                else
                    _int128 = BinaryPrimitives.ReverseEndianness(value);
            }
        }
        public Int128 Int128ValueBE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return BinaryPrimitives.ReverseEndianness(_int128);
                else
                    return _int128;
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _int128 = BinaryPrimitives.ReverseEndianness(value);
                else
                    _int128 = value;
            }
        }

        [FieldOffset(0)] public UInt128 _uint128;
        public UInt128 UInt128ValueLE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return _uint128;
                else
                    return BinaryPrimitives.ReverseEndianness(_uint128);
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _uint128 = value;
                else
                    _uint128 = BinaryPrimitives.ReverseEndianness(value);
            }
        }
        public UInt128 UInt128ValueBE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return BinaryPrimitives.ReverseEndianness(_uint128);
                else
                    return _uint128;
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _uint128 = BinaryPrimitives.ReverseEndianness(value);
                else
                    _uint128 = value;
            }
        }
#endif
    }

}