using Shouldly;
using System;
using System.Linq;
using System.Numerics;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class BlockTests
    {
        private T CopyAndCompare<T>(T orig, int size) where T : struct, IMemBlock, IEquatable<T>
        {
            Span<byte> buffer1 = stackalloc byte[size];
            orig.TryWrite(buffer1).ShouldBeTrue();

            Span<byte> buffer2 = stackalloc byte[size];
            orig.WriteTo(buffer2);
            buffer2.SequenceEqual(buffer1).ShouldBeTrue();

            Span<byte> buffer3 = orig.ToByteArray();
            buffer3.SequenceEqual(buffer1).ShouldBeTrue();

            string origAsStr = orig.ToBase64String();

            T copy = default;
            copy.IsEmpty.ShouldBeTrue();
            copy.TryRead(buffer1).ShouldBeTrue();
            copy.ShouldBe(orig);
            copy.Equals(orig).ShouldBeTrue();
            copy.GetHashCode().ShouldBe(orig.GetHashCode());
            Equals(copy, orig).ShouldBeTrue();

            string copyAsStr = copy.ToBase64String();
            copyAsStr.ShouldBe(origAsStr);

            return copy;
        }

        [Fact]
        public void CopyBlockB001()
        {
            BlockB001 orig = default;
            orig.IsEmpty.ShouldBeTrue();
            orig.ByteValue = 0xFF;
            var copy = CopyAndCompare(orig, 1);
            copy.ByteValue.ShouldBe((byte)0xFF);
        }

        [Theory]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        [InlineData((short)1)]
        [InlineData(default(short))]
        public void CopyBlockB002(short value)
        {
            BlockB002 orig = default;
            orig.IsEmpty.ShouldBeTrue();
            orig.Int16ValueBE = value;
            var copy = CopyAndCompare(orig, 2);
            copy.Int16ValueBE.ShouldBe(value);
        }

        [Theory]
        [InlineData(float.MaxValue)]
        [InlineData(float.MinValue)]
        [InlineData(default(float))]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.Epsilon)]
        [InlineData(float.NaN)]
#if NET7_0_OR_GREATER
        [InlineData(float.E)]
        [InlineData(float.Pi)]
        [InlineData(float.Tau)]
#endif
        public void CopyBlockB004(float value)
        {
            BlockB004 orig = default;
            orig.IsEmpty.ShouldBeTrue();
            orig.SingleValueBE = value;
            var copy = CopyAndCompare(orig, 4);
            copy.SingleValueBE.ShouldBe(value);
        }

        [Theory]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        [InlineData(default(double))]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.Epsilon)]
        [InlineData(double.NaN)]
#if NET7_0_OR_GREATER
        [InlineData(double.E)]
        [InlineData(double.Pi)]
        [InlineData(double.Tau)]
#endif
        public void CopyBlockB008(double value)
        {
            BlockB008 orig = default;
            orig.IsEmpty.ShouldBeTrue();
            orig.DoubleValueBE = value;
            var copy = CopyAndCompare(orig, 8);
            copy.DoubleValueBE.ShouldBe(value);
        }

        [Theory]
        [InlineData("empty")]
        [InlineData("other")]
        public void CopyBlockB016_GuidLE(string input)
        {
            Guid value = input switch
            {
                "empty" => Guid.Empty,
                "other" => Guid.NewGuid(),
                _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
            };
            BlockB016 orig = default;
            orig.IsEmpty.ShouldBeTrue();
            orig.GuidValueLE = value;
            var copy = CopyAndCompare(orig, 16);
            copy.GuidValueLE.ShouldBe(value);
        }

        [Theory]
        [InlineData("empty")]
        [InlineData("other")]
        public void CopyBlockB016_GuidBE(string input)
        {
            Guid value = input switch
            {
                "empty" => Guid.Empty,
                "other" => Guid.NewGuid(),
                _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
            };
            BlockB016 orig = default;
            orig.IsEmpty.ShouldBeTrue();
            orig.GuidValueBE = value;
            var copy = CopyAndCompare(orig, 16);
            copy.GuidValueBE.ShouldBe(value);
        }

        [Theory]
        [InlineData("def")]
        [InlineData("zero")]
        [InlineData("real1")]
        [InlineData("imag1")]
#if NET7_0_OR_GREATER
        [InlineData("nan")]
#endif
        public void CopyBlockB016_Complex_ViaPairOfInt64(string test)
        {
            Complex sendValue = test switch
            {
                "def" => default,
                "zero" => Complex.Zero,
                "real1" => Complex.One,
                "imag1" => Complex.ImaginaryOne,
#if NET7_0_OR_GREATER
                "nan" => Complex.NaN,
#endif
                _ => throw new ArgumentOutOfRangeException(nameof(test), test, null)
            };

            PairOfInt64 wireValue = new PairOfInt64(BitConverter.DoubleToInt64Bits(sendValue.Real), BitConverter.DoubleToInt64Bits(sendValue.Imaginary));

            Complex recdValue = new Complex(BitConverter.Int64BitsToDouble(wireValue.A), BitConverter.Int64BitsToDouble(wireValue.B));

            recdValue.ShouldBe(sendValue);
            recdValue.Real.ShouldBe(sendValue.Real);
            recdValue.Imaginary.ShouldBe(sendValue.Imaginary);
        }

#if NET7_0_OR_GREATER
        [Theory]
        [InlineData("def")]
        [InlineData("zero")]
        [InlineData("real1")]
        [InlineData("imag1")]
        [InlineData("nan")]
        public void CopyBlockB016_Complex_ViaInt128(string test)
        {
            Complex sendValue = test switch
            {
                "def" => default,
                "zero" => Complex.Zero,
                "real1" => Complex.One,
                "imag1" => Complex.ImaginaryOne,
                "nan" => Complex.NaN,
                _ => throw new ArgumentOutOfRangeException(nameof(test), test, null)
            };

            Int128 wireValue;
            {
                BlockB016 orig = default;
                orig.A.UInt64ValueLE = BitConverter.DoubleToUInt64Bits(sendValue.Real);
                orig.B.UInt64ValueLE = BitConverter.DoubleToUInt64Bits(sendValue.Imaginary);
                wireValue = orig.Int128ValueLE;
            }

            Complex recdValue;
            {
                BlockB016 copy = default;
                copy.Int128ValueLE = wireValue;
                recdValue = new Complex(
                    BitConverter.UInt64BitsToDouble(copy.A.UInt64ValueLE),
                    BitConverter.UInt64BitsToDouble(copy.B.UInt64ValueLE)
                );
            }
            recdValue.ShouldBe(sendValue);
            recdValue.Real.ShouldBe(sendValue.Real);
            recdValue.Imaginary.ShouldBe(sendValue.Imaginary);
        }
#endif

        [Fact]
        public void CopyBlockB032()
        {
            BlockB032 orig = default;
            ReadOnlySpan<byte> data = Enumerable.Range(0, orig.BlockSize).Select(i => (byte)(i % 256)).ToArray();
            orig.TryRead(data).ShouldBeTrue();
            var copy = CopyAndCompare(orig, orig.BlockSize);
        }

        [Fact]
        public void CopyBlockB064()
        {
            BlockB064 orig = default;
            ReadOnlySpan<byte> data = Enumerable.Range(0, orig.BlockSize).Select(i => (byte)(i % 256)).ToArray();
            orig.TryRead(data).ShouldBeTrue();
            var copy = CopyAndCompare(orig, orig.BlockSize);
        }

        [Fact]
        public void CopyBlockB128()
        {
            BlockB128 orig = default;
            ReadOnlySpan<byte> data = Enumerable.Range(0, orig.BlockSize).Select(i => (byte)(i % 256)).ToArray();
            orig.TryRead(data).ShouldBeTrue();
            var copy = CopyAndCompare(orig, orig.BlockSize);
        }

    }
}