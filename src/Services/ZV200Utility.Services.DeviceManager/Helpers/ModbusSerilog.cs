using Microsoft.Extensions.Logging;
using NModbus;
using NModbus.Logging;

namespace ZV200Utility.Services.DeviceManager.Helpers
{
    /// <inheritdoc />
    public class ModbusSerilog : ModbusLogger
    {
        private readonly ILogger _logger;

        /// <inheritdoc />
        public ModbusSerilog(LoggingLevel minimumLoggingLevel, ILogger logger)
            : base(minimumLoggingLevel)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        protected override void LogCore(LoggingLevel level, string message)
        {
            _logger.LogInformation("{@Message}\n", message);
        }
    }
}
