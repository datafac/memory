using DataFac.UnsafeHelpers;
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

        public int BlockSize => Size;

        [FieldOffset(0)] public BlockB016 A;
        [FieldOffset(16)] public BlockB016 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, Size), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, Size),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif

        public string UTF8String
        {
            get => DataFac.UnsafeHelpers.BlockHelper.GetString(ref this);
            set => DataFac.UnsafeHelpers.BlockHelper.SetString(ref this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BlockB032 other)
        {
            var self = BlockHelper.AsReadOnlySpanOfInt64(ref this);
            var that = BlockHelper.AsReadOnlySpanOfInt64(ref other);
            return self.SequenceEqual<long>(that);
        }
        public override bool Equals(object? obj) => obj is BlockB032 other && Equals(other);
        public override int GetHashCode()
        {
            var self = BlockHelper.AsReadOnlySpan(ref this);
            HashCode hashCode = new HashCode();
            hashCode.Add(self.Length);
#if NET8_0_OR_GREATER
            hashCode.AddBytes(self);
#else
            for (int i = 0; i < self.Length; i++)
            {
                hashCode.Add(self[i]);
            }
#endif
            return hashCode.ToHashCode();
        }
    }

}