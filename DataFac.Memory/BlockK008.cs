using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 8 * 1024)]
    public struct BlockK008
    {
        [FieldOffset(0)]
        public BlockK004 A;
        [FieldOffset(4 * 1024)]
        public BlockK004 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, 8 * 1024), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, 8 * 1024),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif
    }

}