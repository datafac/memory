using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 128)]
    public struct BlockB128 : IMemBlock
    {
        [FieldOffset(0)]
        public BlockB064 A;
        [FieldOffset(64)]
        public BlockB064 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, 128), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, 128),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif
    }

}