using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public readonly struct PairOfInt16 : IEquatable<PairOfInt16>
    {
        [FieldOffset(0)] public readonly Int16 A;
        [FieldOffset(2)] public readonly Int16 B;
        public PairOfInt16(Int16 a, Int16 b)
        {
            A = a;
            B = b;
        }

        public bool Equals(PairOfInt16 other) => A == other.A && B == other.B;
        public override bool Equals(object? obj) => obj is PairOfInt16 other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(A, B);
        override public string ToString() => $"({A},{B})";
        public static bool operator ==(PairOfInt16 left, PairOfInt16 right) => left.Equals(right);
        public static bool operator !=(PairOfInt16 left, PairOfInt16 right) => !left.Equals(right);
    }

}