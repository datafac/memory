using System;
using System.Runtime.CompilerServices;

namespace DataFac.UnsafeHelpers
{
    public static class BlockHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BlockSize<T>() where T : struct
        {
            return Unsafe.SizeOf<T>();
        }

        public static bool AreAllZero(this ReadOnlySpan<long> source)
        {
            for (var i = 0; i < source.Length; i++)
            {
                if (source[i] != 0) return false;
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlySpan<byte> AsReadOnlySpan<T>(ref T source) where T : struct
        {
            int size = Unsafe.SizeOf<T>();
            return new ReadOnlySpan<byte>(Unsafe.AsPointer(ref source), size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlySpan<long> AsReadOnlySpanOfInt64<TFrom>(ref TFrom source) where TFrom : struct
        {
            int size = Unsafe.SizeOf<TFrom>() / 8;
            var span = new ReadOnlySpan<long>(Unsafe.AsPointer(ref source), size);
            return span;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Span<byte> AsWritableSpan<T>(ref T target) where T : struct
        {
            int size = Unsafe.SizeOf<T>();
            return new Span<byte>(Unsafe.AsPointer(ref target), size);
        }
    }
}
