using System;
using System.Drawing;
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
                    > 0 and < Size => Encoding.UTF8.GetString(source.Slice(1, length)),
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
                        : (byte)(Encoding.UTF8.GetBytes(value.AsSpan(), target.Slice(1)));
            }
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BlockB064 other) => this.A.Equals(other.A) && this.B.Equals(other.B);
    }

}