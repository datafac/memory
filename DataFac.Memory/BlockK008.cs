using DataFac.UnsafeHelpers;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 8 * 1024)]
    public struct BlockK008 : IMemBlock, IEquatable<BlockK008>
    {
        private const int Size = 8 * 1024;

        public int BlockSize => Size;

        [FieldOffset(0)]
        public BlockK004 A;
        [FieldOffset(4 * 1024)]
        public BlockK004 B;

        public bool TryRead(ReadOnlySequence<byte> source)
        {
            var span = BlockHelper.AsWritableSpan(ref this);
            if (source.IsEmpty) return false;
            if (source.IsSingleSegment)
            {
                var segment = source.First;
                if (segment.Length < Size) return false;
                segment.Span.Slice(0, Size).CopyTo(span);
                return true;
            }
            int bytesRemaining = Size;
            foreach (var segment in source)
            {
                if (bytesRemaining == 0) break;
                if (segment.Length > bytesRemaining)
                {
                    segment.Span.Slice(0, bytesRemaining).CopyTo(span);
                    span = span.Slice(bytesRemaining);
                    bytesRemaining = 0;
                }
                else
                {
                    segment.Span.CopyTo(span);
                    span = span.Slice(segment.Length);
                    bytesRemaining -= segment.Length;
                }
            }
            return bytesRemaining == 0;
        }

        public bool TryRead(ReadOnlySpan<byte> source)
        {
            if (source.Length < Size) return false;
            var span = BlockHelper.AsWritableSpan(ref this);
            source.Slice(0, Size).CopyTo(span);
            return true;
        }

        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, Size),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif

        public void WriteTo(Span<byte> target) => BlockHelper.AsReadOnlySpan(ref this).CopyTo(target);
        public void WriteTo(int start, int length, Span<byte> target) => BlockHelper.AsReadOnlySpan(ref this).Slice(start, length).CopyTo(target);

        public string ToBase64String(Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            var span = BlockHelper.AsReadOnlySpan(ref this);
#if NET8_0_OR_GREATER
            return Convert.ToBase64String(span, options);
#else
            return Convert.ToBase64String(span.ToArray(), options);
#endif
        }

        public string ToBase64String(int start, int length, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            var span = BlockHelper.AsReadOnlySpan(ref this).Slice(start, length);
#if NET8_0_OR_GREATER
            return Convert.ToBase64String(span, options);
#else
            return Convert.ToBase64String(span.ToArray(), options);
#endif
        }

        public byte[] ToByteArray() => BlockHelper.AsReadOnlySpan(ref this).ToArray();
        public byte[] ToByteArray(int start, int length) => BlockHelper.AsReadOnlySpan(ref this).Slice(start, length).ToArray();

        public string UTF8String
        {
            get => BlockHelper.GetString(ref this);
            set => BlockHelper.SetString(ref this, value);
        }

        public bool IsEmpty => BlockHelper.AsReadOnlySpanOfInt64(ref this).AreAllZero();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BlockK008 other) => BlockHelper.AsReadOnlySpanOfInt64(ref this).SequenceEqual(BlockHelper.AsReadOnlySpanOfInt64(ref other));
        public override bool Equals(object? obj) => obj is BlockK008 other && Equals(other);
        public override int GetHashCode()
        {
            HashCode hashCode = new HashCode();
            hashCode.Add(Size);
#if NET8_0_OR_GREATER
            hashCode.AddBytes(BlockHelper.AsReadOnlySpan(ref this));
#else
            var self = BlockHelper.AsReadOnlySpanOfInt64(ref this);
            for (int i = 0; i < self.Length; i++)
            {
                hashCode.Add(self[i]);
            }
#endif
            return hashCode.ToHashCode();
        }
    }

}