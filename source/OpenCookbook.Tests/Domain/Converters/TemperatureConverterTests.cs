using OpenCookbook.Domain.Converters;

namespace OpenCookbook.Tests.Domain.Converters
{
    public class TemperatureConverterTests
    {
        [Theory]
        [InlineData(32, 32, "C", "C")]
        [InlineData(10, 10, "F", "F")]
        public void SameUnits_IsValid_Succeeds(double expected, double input, string source, string target)
        {
            var converter = new TemperatureConverter();

            var result = converter.Convert(input, source, target);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(32, 0, "C", "F")]
        [InlineData(50, 10, "C", "F")]
        [InlineData(14, -10, "C", "F")]
        public void CelciusToFahrenheit_IsValid_Succeeds(double expected, double input, string source, string target)
        {
            var converter = new TemperatureConverter();

            var result = converter.Convert(input, source, target);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-17.78, 0, "F", "C")]
        [InlineData(0, 32, "F", "C")]
        [InlineData(37.22, 99, "F", "C")]
        public void FahrenheitToCelcius_IsValid_Succeeds(double expected, double input, string source, string target)
        {
            var converter = new TemperatureConverter();

            var result = converter.Convert(input, source, target);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Source_IsInValid_ThrowsException()
        {
            var converter = new TemperatureConverter();

            Assert.Throws<ArgumentException>("source", () => { converter.Convert(0, "S", "C"); });
        }

        [Fact]
        public void SourceFTarget_IsInValid_ThrowsException()
        {
            var converter = new TemperatureConverter();

            Assert.Throws<ArgumentException>("target", () => { converter.Convert(0, "F", "T"); });
        }

        [Fact]
        public void SourceCTarget_IsInValid_ThrowsException()
        {
            var converter = new TemperatureConverter();

            Assert.Throws<ArgumentException>("target", () => { converter.Convert(0, "C", "T"); });
        }
    }
}