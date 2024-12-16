using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public struct BlockB032 : IMemBlock, IEquatable<BlockB032>
    {
        private const int Size = 32;

        [FieldOffset(0)] public BlockB016 A;
        [FieldOffset(16)] public BlockB016 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, Size), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, Size),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif

        public string? UTF8String
        {
            get => DataFac.UnsafeHelpers.BlockHelper.GetString(ref this);
            set => DataFac.UnsafeHelpers.BlockHelper.SetString(ref this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BlockB032 other) => this.A.Equals(other.A) && this.B.Equals(other.B);
    }

}