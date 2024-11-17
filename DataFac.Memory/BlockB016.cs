using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct BlockB016 : IMemBlock
    {
        [FieldOffset(0)] public BlockB008 A;
        [FieldOffset(8)] public BlockB008 B;

        public bool TryRead(ReadOnlySpan<byte> source) => MemoryMarshal.TryRead(source.Slice(0, 16), out this);
        public bool TryWrite(Span<byte> target) => MemoryMarshal.TryWrite(target.Slice(0, 16),
#if NET8_0_OR_GREATER
            in this);
#else
            ref this);
#endif

        [FieldOffset(0)] public Guid _guid;
        public Guid GuidValueLE
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                    return _guid;
                else
                    throw new NotImplementedException();
            }
            set
            {
                if (BitConverter.IsLittleEndian)
                    _guid = value;
                else
                    throw new NotImplementedException();
            }
        }
    }

}