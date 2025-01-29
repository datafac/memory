using DataFac.UnsafeHelpers;
using System;
using System.Buffers.Binary;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public struct BlockB064 : IMemBlock, IEquatable<BlockB064>
    {
        private const int Size = 64;

        public int BlockSize => Size;

        [FieldOffset(0)] public BlockB032 A;
        [FieldOffset(32)] public BlockB032 B;

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
        public bool Equals(BlockB064 other)
        {
            var self = BlockHelper.AsReadOnlySpanOfInt64(ref this);
            var that = BlockHelper.AsReadOnlySpanOfInt64(ref other);
            return self.SequenceEqual<long>(that);
        }
        public override bool Equals(object? obj) => obj is BlockB064 other && Equals(other);
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

        public void GetInt32ArrayBE(Span<Int32> target)
        {
            var source = MemoryMarshal.Cast<byte, Int32>(BlockHelper.AsReadOnlySpan(ref this));
            if (!BitConverter.IsLittleEndian)
            {
                source.Slice(0, target.Length).CopyTo(target);
            }
            else
            {
                for (int i = 0; i < target.Length; i++)
                {
                    target[i] = BinaryPrimitives.ReverseEndianness(source[i]);
                }
            }
        }
        public void SetInt32ArrayBE(ReadOnlySpan<Int32> values)
        {
            var target = MemoryMarshal.Cast<byte, Int32>(BlockHelper.AsWritableSpan(ref this));
            if (!BitConverter.IsLittleEndian)
            {
                values.CopyTo(target);
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    target[i] = BinaryPrimitives.ReverseEndianness(values[i]);
                }
            }
        }
        public void GetInt32ArrayLE(Span<Int32> target)
        {
            var source = MemoryMarshal.Cast<byte, Int32>(BlockHelper.AsReadOnlySpan(ref this));
            if (BitConverter.IsLittleEndian)
            {
                source.Slice(0, target.Length).CopyTo(target);
            }
            else
            {
                for (int i = 0; i < target.Length; i++)
                {
                    target[i] = BinaryPrimitives.ReverseEndianness(source[i]);
                }
            }
        }
        public void SetInt32ArrayLE(ReadOnlySpan<Int32> values)
        {
            var target = MemoryMarshal.Cast<byte, Int32>(BlockHelper.AsWritableSpan(ref this));
            if (BitConverter.IsLittleEndian)
            {
                values.CopyTo(target);
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    target[i] = BinaryPrimitives.ReverseEndianness(values[i]);
                }
            }
        }
        public void GetUInt32ArrayBE(Span<UInt32> target)
        {
            var source = MemoryMarshal.Cast<byte, UInt32>(BlockHelper.AsReadOnlySpan(ref this));
            if (!BitConverter.IsLittleEndian)
            {
                source.Slice(0, target.Length).CopyTo(target);
            }
            else
            {
                for (int i = 0; i < target.Length; i++)
                {
                    target[i] = BinaryPrimitives.ReverseEndianness(source[i]);
                }
            }
        }
        public void SetUInt32ArrayBE(ReadOnlySpan<UInt32> values)
        {
            var target = MemoryMarshal.Cast<byte, UInt32>(BlockHelper.AsWritableSpan(ref this));
            if (!BitConverter.IsLittleEndian)
            {
                values.CopyTo(target);
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    target[i] = BinaryPrimitives.ReverseEndianness(values[i]);
                }
            }
        }
        public void GetUInt32ArrayLE(Span<UInt32> target)
        {
            var source = MemoryMarshal.Cast<byte, UInt32>(BlockHelper.AsReadOnlySpan(ref this));
            if (BitConverter.IsLittleEndian)
            {
                source.Slice(0, target.Length).CopyTo(target);
            }
            else
            {
                for (int i = 0; i < target.Length; i++)
                {
                    target[i] = BinaryPrimitives.ReverseEndianness(source[i]);
                }
            }
        }
        public void SetUInt32ArrayLE(ReadOnlySpan<UInt32> values)
        {
            var target = MemoryMarshal.Cast<byte, UInt32>(BlockHelper.AsWritableSpan(ref this));
            if (BitConverter.IsLittleEndian)
            {
                values.CopyTo(target);
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    target[i] = BinaryPrimitives.ReverseEndianness(values[i]);
                }
            }
        }
    }
}