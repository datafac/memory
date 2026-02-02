using DataFac.UnsafeHelpers;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public struct BlockB001 : IMemBlock, IEquatable<BlockB001>
    {
        private const int Size = 1;

        public int BlockSize => Size;

        [FieldOffset(0)] public bool BoolValue;
        [FieldOffset(0)] public sbyte SByteValue;
        [FieldOffset(0)] public byte ByteValue;

        public string UTF8String
        {
            get => BlockHelper.GetString(ref this);
            set => BlockHelper.SetString(ref this, value);
        }

        public bool IsEmpty => ByteValue == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BlockB001 other) => ByteValue == other.ByteValue;
        public override bool Equals(object? obj) => obj is BlockB001 other && Equals(other);
        public override int GetHashCode() => ByteValue.GetHashCode();

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

    }

}