using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public struct BlockB001 : IMemBlock, IEquatable<BlockB001>
    {
        [FieldOffset(0)] public bool BoolValue;
        [FieldOffset(0)] public sbyte SByteValue;
        [FieldOffset(0)] public byte ByteValue;

        public bool Equals(BlockB001 other) => ByteValue == other.ByteValue;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, 1), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, 1),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif
    }

}