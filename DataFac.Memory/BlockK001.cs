using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 1 * 1024)]
    public struct BlockK001 : IMemBlock
    {
        [FieldOffset(0)]
        public BlockB512 A;
        [FieldOffset(512)]
        public BlockB512 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, 1 * 1024), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, 1 * 1024),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif
    }

}