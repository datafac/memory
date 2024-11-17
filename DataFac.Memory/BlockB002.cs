using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct BlockB002 : IMemBlock, IEquatable<BlockB002>
    {
        [FieldOffset(0)] public BlockB001 A;
        [FieldOffset(1)] public BlockB001 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, 2), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, 2),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);

#endif

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