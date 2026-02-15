using System;
using System.Runtime.InteropServices;

namespace DataFac.Memory;

[StructLayout(LayoutKind.Explicit, Size = 16)]
public readonly struct QuadOfInt32 : IEquatable<QuadOfInt32>
{
    [FieldOffset(0)] public readonly Int32 A;
    [FieldOffset(4)] public readonly Int32 B;
    [FieldOffset(8)] public readonly Int32 C;
    [FieldOffset(12)] public readonly Int32 D;
    public QuadOfInt32(Int32 a, Int32 b, Int32 c, Int32 d)
    {
        A = a;
        B = b;
        C = c;
        D = d;
    }

    public bool Equals(QuadOfInt32 other) => A == other.A && B == other.B && C == other.C && D == other.D;
    public override bool Equals(object? obj) => obj is QuadOfInt32 other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(A, B, C, D);
    override public string ToString() => $"({A},{B},{C},{D})";
    public static bool operator ==(QuadOfInt32 left, QuadOfInt32 right) => left.Equals(right);
    public static bool operator !=(QuadOfInt32 left, QuadOfInt32 right) => !left.Equals(right);
}