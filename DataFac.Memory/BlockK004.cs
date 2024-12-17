using DataFac.UnsafeHelpers;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 4 * 1024)]
    public struct BlockK004 : IMemBlock, IEquatable<BlockK004>
    {
        private const int Size = 4 * 1024;

        [FieldOffset(0)]
        public BlockK002 A;
        [FieldOffset(2 * 1024)]
        public BlockK002 B;

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
        public bool Equals(BlockK004 other) => this.A.Equals(other.A) && this.B.Equals(other.B);

    }

}