﻿using Shouldly;
using System;
using Xunit;

namespace DataFac.Memory.Tests
{
    public class BlockTests
    {
        private T CopyAndCompare<T>(T orig, int size) where T : struct, IMemBlock, IEquatable<T>
        {
            Span<byte> buffer = stackalloc byte[size];

            orig.TryWrite(buffer).ShouldBeTrue();

            T copy = default;
            copy.TryRead(buffer).ShouldBeTrue();

            copy.ShouldBe(orig);
            copy.Equals(orig).ShouldBeTrue();

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
        public void CopyBlockB016(string input)
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

    }
}