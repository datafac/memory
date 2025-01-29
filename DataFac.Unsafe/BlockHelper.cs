using System;
using System.Buffers.Binary;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static string GetStringFromSpan(ReadOnlySpan<byte> source)
        {
            fixed (byte* sourcePtr = source)
            {
                int blockSize = source.Length;
                if (blockSize <= 256)
                {
                    byte length = source[0];
                    return length == 0 
                        ? string.Empty 
                        : Encoding.UTF8.GetString(sourcePtr + 1, length);
                }
                else
                {
                    short length = BinaryPrimitives.ReadInt16LittleEndian(source.Slice(0, 2));
                    return length == 0 
                        ? string.Empty 
                        : Encoding.UTF8.GetString(sourcePtr + 2, length);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static string GetString<T>(ref T source) where T : struct
        {
            int blockSize = Unsafe.SizeOf<T>();
            byte* pointer = (byte*)(Unsafe.AsPointer<T>(ref source));
            var span = new ReadOnlySpan<byte>(pointer, blockSize);
            return GetStringFromSpan(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void SetStringIntoSpan(Span<byte> target, string? value)
        {
            target.Clear();
            if (value is null) return;

            fixed (byte* targetPtr = target)
            {
                int blockSize = target.Length;
                int valueLen = value.Length;
                if (blockSize <= 256)
                {
                    int maxByteCount = blockSize - 1;
                    if (valueLen == 0)
                    {
                        target[0] = 0;
                    }
                    else
                    {
                        fixed (char* valuePtr = value)
                        {
                            int bytesWritten = Encoding.UTF8.GetBytes(valuePtr, valueLen, targetPtr + 1, maxByteCount);
                            target[0] = (byte)bytesWritten;
                        }
                    }
                }
                else
                {
                    int maxByteCount = blockSize - 2;
                    if (valueLen == 0)
                    {
                        BinaryPrimitives.WriteInt16LittleEndian(target.Slice(0, 2), 0);
                    }
                    else
                    {
                        fixed (char* valuePtr = value)
                        {
                            int bytesWritten = Encoding.UTF8.GetBytes(valuePtr, valueLen, targetPtr + 2, maxByteCount);
                            BinaryPrimitives.WriteInt16LittleEndian(target.Slice(0, 2), (short)bytesWritten);
                        }
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void SetString<T>(ref T target, string? value) where T : struct
        {
            var blockSize = Unsafe.SizeOf<T>();
            var pointer = (byte*)(Unsafe.AsPointer<T>(ref target));
            var span = new Span<byte>(pointer, blockSize);
            SetStringIntoSpan(span, value);
        }
    }
}
