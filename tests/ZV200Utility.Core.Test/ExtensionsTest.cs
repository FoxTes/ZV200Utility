using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NModbus;
using Xunit;
using ZV200Utility.Core.Enums;
using ZV200Utility.Services.DeviceManager.Extensions;

namespace ZV200Utility.Core.Test
{
    public class ExtensionsTest
    {
        [Theory]
        [InlineData(RegisterAddress.BusA, RegisterAddress.BusC, new[] {0, 1, 2} )]
        [InlineData(RegisterAddress.RelayFunction, RegisterAddress.InputDiscreteLogic, new[] {0, 1, 0, 1} )]
        [InlineData(RegisterAddress.BusA, RegisterAddress.Version, new[] {0, 1, 2, 3, 0, 1, 0} )]
        public async void ModbusExtensionTest(
            RegisterAddress startRegister, 
            RegisterAddress stopRegister,
            int[] outArray)
        {
            var mock = new Mock<IModbusSerialMaster>();
            mock.Setup(x => x.ReadHoldingRegistersAsync(It.IsAny<byte>(), It.IsAny<ushort>(), It.IsAny<ushort>()))
                .Returns((byte slaveAddress, ushort startAddress, ushort count) 
                    => Task.FromResult(Enumerable.Range(0, count).Select(x => (ushort)x).ToArray()));

            var result = await mock.Object.ReadHoldingRegisterRanges(startRegister, stopRegister);
            result.ToArray().Length.Should().Be(outArray.Length);
            result.ToArray().Should().Equal(outArray);
        }
    }
}
