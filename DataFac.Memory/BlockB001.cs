using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public struct BlockB001 : IMemBlock, IEquatable<BlockB001>
    {
        private const int Size = 1;

        [FieldOffset(0)] public bool BoolValue;
        [FieldOffset(0)] public sbyte SByteValue;
        [FieldOffset(0)] public byte ByteValue;

#if NET6_0_OR_GREATER
        [FieldOffset(0)] private byte _marker;
        public string? UTF8String
        {
            get
            {
                ReadOnlySpan<byte> source = MemoryMarshal.CreateReadOnlySpan<byte>(ref _marker, Size);
                byte length = source[0];
                return length switch
                {
                    0xFF => null,
                    0 => string.Empty,
                    _ => throw new InvalidDataException($"Invalid string length ({length}). Length must be < {Size}.")
                };
            }

            set
            {
                Span<byte> target = MemoryMarshal.CreateSpan<byte>(ref _marker, Size);
                target.Clear();
                target[0] = value is null
                    ? (byte)0xFF
                    : value.Length == 0
                        ? (byte)0
                        : throw new InvalidDataException($"Invalid string length ({value.Length}). Length must be < {Size}.");
            }
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BlockB001 other) => ByteValue == other.ByteValue;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, Size), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, Size),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif
    }

}