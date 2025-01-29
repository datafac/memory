﻿using DataFac.UnsafeHelpers;
using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct BlockB004 : IMemBlock, IEquatable<BlockB004>
    {
        private const int Size = 4;

        public int BlockSize => Size;

        [FieldOffset(0)] public BlockB002 A;
        [FieldOffset(2)] public BlockB002 B;

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

        public bool IsEmpty => _int == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BlockB004 other) => _int == other._int;

        [FieldOffset(0)] public int _int;
        public int Int32ValueLE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return _int;
                else
                    return BinaryPrimitives.ReverseEndianness(_int);
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _int = value;
                else
                    _int = BinaryPrimitives.ReverseEndianness(value);
            }
        }
        public int Int32ValueBE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return BinaryPrimitives.ReverseEndianness(_int);
                else
                    return _int;
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _int = BinaryPrimitives.ReverseEndianness(value);
                else
                    _int = value;
            }
        }

        [FieldOffset(0)] public float _float;
        public float SingleValueLE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return _float;
                else
                {
#if NET6_0_OR_GREATER
                    return BitConverter.Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(_int));
#else
                    return new Convert32(BinaryPrimitives.ReverseEndianness(_int)).FloatValue;
#endif
                }
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _float = value;
                else
                {
#if NET6_0_OR_GREATER
                    _int = BinaryPrimitives.ReverseEndianness(BitConverter.SingleToInt32Bits(value));
#else
                    _int = BinaryPrimitives.ReverseEndianness(new Convert32(value).Int32Value);
#endif
                }
            }
        }
        public float SingleValueBE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                {
#if NET6_0_OR_GREATER
                    return BitConverter.Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(_int));
#else
                    return new Convert32(BinaryPrimitives.ReverseEndianness(_int)).FloatValue;
#endif
                }
                else
                    return _float;
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                {
#if NET6_0_OR_GREATER
                    _int = BinaryPrimitives.ReverseEndianness(BitConverter.SingleToInt32Bits(value));
#else
                    _int = BinaryPrimitives.ReverseEndianness(new Convert32(value).Int32Value);
#endif
                }
                else
                    _float = value;
            }
        }
    }

}