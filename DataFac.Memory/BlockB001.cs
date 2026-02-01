using DataFac.UnsafeHelpers;
using System;
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

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, Size), out this);
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