﻿using DataFac.UnsafeHelpers;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 1 * 1024)]
    public struct BlockK001 : IMemBlock, IEquatable<BlockK001>
    {
        private const int Size = 1 * 1024;

        public int BlockSize => Size;

        [FieldOffset(0)]
        public BlockB512 A;
        [FieldOffset(512)]
        public BlockB512 B;

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

        public bool IsEmpty => BlockHelper.AsReadOnlySpanOfInt64(ref this).AreAllZero();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BlockK001 other)
        {
            var self = BlockHelper.AsReadOnlySpanOfInt64(ref this);
            var that = BlockHelper.AsReadOnlySpanOfInt64(ref other);
            return self.SequenceEqual<long>(that);
        }
        public override bool Equals(object? obj) => obj is BlockK001 other && Equals(other);
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