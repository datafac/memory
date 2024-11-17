using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 256)]
    public struct BlockB256 : IMemBlock
    {
        [FieldOffset(0)]
        public BlockB128 A;
        [FieldOffset(128)]
        public BlockB128 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, 256), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, 256),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif
    }

}