using DataFac.UnsafeHelpers;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 128)]
    public struct BlockB128 : IMemBlock, IEquatable<BlockB128>
    {
        private const int Size = 128;

        [FieldOffset(0)]
        public BlockB064 A;
        [FieldOffset(64)]
        public BlockB064 B;

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
        public bool Equals(BlockB128 other) => this.A.Equals(other.A) && this.B.Equals(other.B);

    }

}