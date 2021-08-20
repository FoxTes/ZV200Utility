using System;
using System.Buffers;
using System.Threading.Tasks;
using FastEnumUtility;
using MemoryPools.Collections;
using NModbus;
using ZV200Utility.Core.Enums;

namespace ZV200Utility.Services.DeviceManager.Extensions
{
    /// <summary>
    /// Расширение, предоставляющие методы для группового чтения регистров.
    /// </summary>
    public static class ModbusExtension
    {
        /// <summary>
        /// Асинхронно читает разделенные на группы регистры хранения.
        /// </summary>
        /// <param name="modbusSerialMaster"><see cref="IModbusSerialMaster"/>.</param>
        /// <param name="startIndex">Стартовый регистр.</param>
        /// <param name="stopIndex">Конечный регистр.</param>
        /// <returns></returns>
        public static async Task<ReadOnlyMemory<int>> ReadHoldingRegisterRanges(
            this IModbusSerialMaster modbusSerialMaster,
            RegisterAddress startIndex,
            RegisterAddress stopIndex)
        {
            var values = FastEnum.GetValues<RegisterAddress>();
            var countValues = values.Count;

            var samePool = ArrayPool<int>.Shared;
            var buffer = samePool.Rent(countValues);

            var y = 0;
            for (var i = 0; i < countValues; i++)
            {
                var value = (int)values[i];
                if (value >= (int)startIndex && value <= (int)stopIndex)
                    buffer[y++] = value;
            }

            var consecutiveRanges = buffer
                .AsPooling()
                .ConsecutiveRanges();
            samePool.Return(buffer);

            var readRegistersBuffer = samePool.Rent(y);
            var z = 0;
            foreach (var (first, count) in consecutiveRanges)
            {
                var registers = await modbusSerialMaster
                    .ReadHoldingRegistersAsync(1, (ushort)first, (ushort)count)
                    .ConfigureAwait(false);
                foreach (var register in registers)
                    readRegistersBuffer[z++] = register;
            }

            var spanArray = new ReadOnlyMemory<int>(readRegistersBuffer, 0, z);
            samePool.Return(readRegistersBuffer);
            return spanArray;
        }
    }
}