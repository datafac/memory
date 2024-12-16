using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 512)]
    public struct BlockB512
    {
        [FieldOffset(0)]
        public BlockB256 A;
        [FieldOffset(256)]
        public BlockB256 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, 512), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, 512),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif
    }

}