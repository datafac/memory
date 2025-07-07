using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public readonly struct PairOfInt32 : IEquatable<PairOfInt32>
    {
        [FieldOffset(0)] public readonly Int32 A;
        [FieldOffset(4)] public readonly Int32 B;
        public PairOfInt32(Int32 a, Int32 b)
        {
            A = a;
            B = b;
        }

        public bool Equals(PairOfInt32 other) => A == other.A && B == other.B;
        public override bool Equals(object? obj) => obj is PairOfInt32 other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(A, B);
        override public string ToString() => $"({A},{B})";
    }

}