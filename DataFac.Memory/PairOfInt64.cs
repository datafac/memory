using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public readonly struct PairOfInt64 : IEquatable<PairOfInt64>
    {
        [FieldOffset(0)] public readonly long A;
        [FieldOffset(8)] public readonly long B;
        public PairOfInt64(long a, long b)
        {
            A = a;
            B = b;
        }

        public bool Equals(PairOfInt64 other) => A == other.A && B == other.B;
        public override bool Equals(object? obj) => obj is PairOfInt64 other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(A, B);
    }

}