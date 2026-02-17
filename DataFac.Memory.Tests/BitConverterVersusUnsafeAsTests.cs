using Shouldly;
using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace DataFac.Memory.Tests;

public class BitConverterVersusUnsafeAsTests
{
    [Theory]
    [InlineData(default(float), 0)]
    [InlineData(1F, 1065353216)]
    [InlineData(-1F, -1082130432)]
    [InlineData(float.MaxValue, 2139095039)]
    [InlineData(float.MinValue, -8388609)]
    [InlineData(float.PositiveInfinity, 2139095040)]
    [InlineData(float.NegativeInfinity, -8388608)]
    [InlineData(float.Epsilon, 1)]
    [InlineData(float.NaN, -4194304)]
#if NET6_0_OR_GREATER
    [InlineData(float.E, 1076754516)]
    [InlineData(float.NegativeZero, -2147483648)]
    [InlineData(float.Pi, 1078530011)]
    [InlineData(float.Tau, 1086918619)]
#endif
    public void Roundtrip_Single(in Single input, Int32 expectedEncoding)
    {
        // encode
        float input2 = input;
        int encoded = Unsafe.As<float, int>(ref input2);
        encoded.ShouldBe(expectedEncoding);

#if NET6_0_OR_GREATER
        // check against BitConverter.SingleToInt32Bits
        int dotnetEncoding = BitConverter.SingleToInt32Bits(input);
        dotnetEncoding.ShouldBe(expectedEncoding);
#endif

        // decode
        float decoded = Unsafe.As<int, float>(ref encoded);
        decoded.ShouldBe(input);

#if NET6_0_OR_GREATER
        // check against BitConverter.Int32BitsToSingle
        float dotnetDecoding = BitConverter.Int32BitsToSingle(encoded);
        dotnetDecoding.ShouldBe(input);
#endif
    }

    [Theory]
    [InlineData(default(double), 0)]
    [InlineData(1D, 4607182418800017408L)]
    [InlineData(-1D, -4616189618054758400L)]
    [InlineData(double.MaxValue, 9218868437227405311L)]
    [InlineData(double.MinValue, -4503599627370497L)]
    [InlineData(double.PositiveInfinity, 9218868437227405312L)]
    [InlineData(double.NegativeInfinity, -4503599627370496L)]
    [InlineData(double.Epsilon, 1)]
    [InlineData(double.NaN, -2251799813685248L)]
#if NET6_0_OR_GREATER
    [InlineData(double.E, 4613303445314885481L)]
    [InlineData(double.NegativeZero, -9223372036854775808L)]
    [InlineData(double.Pi, 4614256656552045848L)]
    [InlineData(double.Tau, 4618760256179416344L)]
#endif
    public void Roundtrip_Double(in Double input, Int64 expectedEncoding)
    {
        // encode
        double input2 = input;
        long encoded = Unsafe.As<double, long>(ref input2);
        encoded.ShouldBe(expectedEncoding);

        // check against BitConverter.DoubleToInt64Bits
        long dotnetEncoding = BitConverter.DoubleToInt64Bits(input);
        dotnetEncoding.ShouldBe(expectedEncoding);

        // decode
        double decoded = Unsafe.As<long, double>(ref encoded);
        decoded.ShouldBe(input);

        // check against BitConverter.Int64BitsToDouble
        double dotnetDecoding = BitConverter.Int64BitsToDouble(encoded);
        dotnetDecoding.ShouldBe(input);
    }
}
