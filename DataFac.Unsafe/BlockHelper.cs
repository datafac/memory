using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlySpan<byte> AsReadOnlySpan<T>(ref T source) where T : struct
        {
            int size = Unsafe.SizeOf<T>();
            return new ReadOnlySpan<byte>(Unsafe.AsPointer(ref source), size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Span<byte> AsWritableSpan<T>(ref T target) where T : struct
        {
            int size = Unsafe.SizeOf<T>();
            return new Span<byte>(Unsafe.AsPointer(ref target), size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static string GetString<T>(ref T source) where T : struct
        {
            int blockSize = Unsafe.SizeOf<T>();
            byte* pointer = (byte*)(Unsafe.AsPointer<T>(ref source));
            var span = new ReadOnlySpan<byte>(pointer, blockSize);
            if (blockSize <= 256)
            {
                byte length = span[0];
                if (length == 0) return string.Empty;
                else
                {
                    return Encoding.UTF8.GetString(pointer + 1, length);
                }
            }
            else
            {
                short length = BinaryPrimitives.ReadInt16LittleEndian(span.Slice(0, 2));
                if (length == 0) return string.Empty;
                else
                {
                    return Encoding.UTF8.GetString(pointer + 2, length);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void SetString<T>(ref T target, string value) where T : struct
        {
            int blockSize = Unsafe.SizeOf<T>();
            byte* pointer = (byte*)(Unsafe.AsPointer<T>(ref target));
            var targetSpan = new Span<byte>(pointer, blockSize);
            targetSpan.Clear();
            if (blockSize <= 256)
            {
                int valueLen = value.Length;
                if (valueLen == 0)
                {
                    targetSpan[0] = 0;
                    return;
                }
                else
                {
                    fixed (char* valuePtr = value)
                    {
                        int maxByteCount = blockSize - 1;
                        int bytesWritten = Encoding.UTF8.GetBytes(valuePtr, valueLen, pointer + 1, maxByteCount);
                        targetSpan[0] = (byte)bytesWritten;
                    }
                }
            }
            else
            {
                int valueLen = value.Length;
                if (valueLen == 0)
                {
                    BinaryPrimitives.WriteInt16LittleEndian(targetSpan.Slice(0, 2), 0);
                    return;
                }
                else
                {
                    fixed (char* valuePtr = value)
                    {
                        int maxByteCount = blockSize - 2;
                        int bytesWritten = Encoding.UTF8.GetBytes(valuePtr, valueLen, pointer + 2, maxByteCount);
                        BinaryPrimitives.WriteInt16LittleEndian(targetSpan.Slice(0, 2), (short)bytesWritten);
                    }
                }
            }
        }

    }
}
