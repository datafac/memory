using DataFac.UnsafeHelpers;
using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct BlockB002 : IMemBlock, IEquatable<BlockB002>
    {
        private const int Size = 2;

        [FieldOffset(0)] public BlockB001 A;
        [FieldOffset(1)] public BlockB001 B;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BlockB002 other) => _short == other._short;

        [FieldOffset(0)] public short _short;
        public short Int16ValueLE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return _short;
                else
                    return BinaryPrimitives.ReverseEndianness(_short);
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _short = value;
                else
                    _short = BinaryPrimitives.ReverseEndianness(value);
            }
        }
        public short Int16ValueBE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return BinaryPrimitives.ReverseEndianness(_short);
                else
                    return _short;
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _short = BinaryPrimitives.ReverseEndianness(value);
                else
                    _short = value;
            }
        }

    }

}