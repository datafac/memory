using DataFac.UnsafeHelpers;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public struct BlockB064 : IMemBlock, IEquatable<BlockB064>
    {
        private const int Size = 64;

        [FieldOffset(0)] public BlockB032 A;
        [FieldOffset(32)] public BlockB032 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, Size), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, Size),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif

        public string? UTF8String
        {
            get => BlockHelper.GetString(ref this);
            set => BlockHelper.SetString(ref this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BlockB064 other) => this.A.Equals(other.A) && this.B.Equals(other.B);
    }

}